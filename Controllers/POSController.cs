using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PositivePOSAPI.Data;
using POSitive.API.Models;
using System.Data;

namespace PositivePOSAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class POSController : ControllerBase
{
    private readonly PositiveDbContext _context;
    private readonly ILogger<POSController> _logger;

    public POSController(PositiveDbContext context, ILogger<POSController> logger)
    {
        _context = context;
        _logger = logger;
    }

    public record NextTicketRequest(Guid LocationGUID, int CompanyID, int SiteID);

    public record CreateHeaderRequest(
        Guid LocationGUID,
        int CompanyID,
        int SiteID,
        string? UserID,
        string? Terminal,
        string? PaymentType,
        string? OrderStatus
    );

    public record AddLineRequest(
        Guid LocationGUID,
        int CompanyID,
        int SiteID,
        int InvoiceNumber,
        string ItemID,
        string? ItemDescription,
        decimal? ItemPrice,
        decimal? Quantity,
        decimal? LineItemTaxAmount,
        int? TaxableInd,
        int? FuelPrepayInd,
        int? FuelFillupInd,
        int? DispenserNumber,
        int? PriceOverride,
        decimal? NonSalePrice,
        int? SpecialInd,
        string? CategoryCode,
        int? SafeDropInd,
        int? DiscountInd,
        string? DiscountCode,
        int? LoyaltyInd,
        string? LoyaltyCode
    );

    public record UpdatePaymentRequest(
        Guid LocationGUID,
        int CompanyID,
        int SiteID,
        int InvoiceNumber,
        decimal? AmountPaid,
        decimal? CashAmount,
        decimal? CheckAmount,
        decimal? CreditAmount,
        string? CheckNumber,
        string? PaymentType,
        string? CardType,
        int? First6,
        int? Last4,
        int? CardExpirationMM,
        int? CardExpirationYY,
        string? ReceiptType,
        string? CardTrack2,
        string? CardResponse
    );

    [HttpPost("next-ticket")]
    public async Task<IActionResult> GetNextTicket([FromBody] NextTicketRequest request)
    {
        if (request.LocationGUID == Guid.Empty)
            return BadRequest("LocationGUID is required.");

        await using var tx = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);

        try
        {
            var ticketRow = await _context.Set<POSTicketNumber>()
                .FirstOrDefaultAsync(x => x.LocationGUID == request.LocationGUID);

            if (ticketRow == null)
            {
                ticketRow = new POSTicketNumber
                {
                    CompanyID = request.CompanyID,
                    SiteID = request.SiteID,
                    LocationGUID = request.LocationGUID,
                    TicketNumber = 1,
                    LastUpdated = DateTime.UtcNow
                };

                _context.Set<POSTicketNumber>().Add(ticketRow);
            }
            else
            {
                ticketRow.TicketNumber += 1;
                ticketRow.LastUpdated = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            await tx.CommitAsync();

            return Ok(new
            {
                ok = true,
                transactionNumber = ticketRow.TicketNumber,
                invoiceNumber = ticketRow.TicketNumber,
                locationGuid = ticketRow.LocationGUID
            });
        }
        catch (Exception ex)
        {
            await tx.RollbackAsync();
            _logger.LogError(ex, "Failed to generate next ticket for {LocationGUID}", request.LocationGUID);
            return StatusCode(500, "Failed to generate next transaction number.");
        }
    }

    [HttpPost("header")]
    public async Task<IActionResult> CreateHeader([FromBody] CreateHeaderRequest request)
    {
        if (request.LocationGUID == Guid.Empty)
            return BadRequest("LocationGUID is required.");

        await using var tx = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);

        try
        {
            var ticketRow = await _context.Set<POSTicketNumber>()
                .FirstOrDefaultAsync(x => x.LocationGUID == request.LocationGUID);

            if (ticketRow == null)
            {
                ticketRow = new POSTicketNumber
                {
                    CompanyID = request.CompanyID,
                    SiteID = request.SiteID,
                    LocationGUID = request.LocationGUID,
                    TicketNumber = 1,
                    LastUpdated = DateTime.UtcNow
                };

                _context.Set<POSTicketNumber>().Add(ticketRow);
            }
            else
            {
                ticketRow.TicketNumber += 1;
                ticketRow.LastUpdated = DateTime.UtcNow;
            }

            var invoiceNumber = ticketRow.TicketNumber;

            var header = new POSTransaction
            {
                CompanyID = request.CompanyID,
                SiteID = request.SiteID,
                LocationGUID = request.LocationGUID,
                InvoiceNumber = invoiceNumber,
                OrderDate = DateTime.UtcNow,
                UserID = request.UserID,
                Terminal = request.Terminal,
                PaymentType = request.PaymentType,
                OrderStatus = string.IsNullOrWhiteSpace(request.OrderStatus) ? "A" : request.OrderStatus,
                Subtotal = 0,
                SalesTax = 0,
                TotalTax = 0,
                TotalAmount = 0,
                AmountPaid = 0,
                AmountBalance = 0
            };

            _context.Set<POSTransaction>().Add(header);

            await _context.SaveChangesAsync();
            await tx.CommitAsync();

            return Ok(new
            {
                ok = true,
                transactionNumber = invoiceNumber,
                invoiceNumber,
                header
            });
        }
        catch (Exception ex)
        {
            await tx.RollbackAsync();
            _logger.LogError(ex, "Failed to create header for {LocationGUID}", request.LocationGUID);
            return StatusCode(500, "Failed to create transaction header.");
        }
    }

    [HttpPost("line")]
    public async Task<IActionResult> AddLine([FromBody] AddLineRequest request)
    {
        if (request.LocationGUID == Guid.Empty)
            return BadRequest("LocationGUID is required.");

        var header = await _context.Set<POSTransaction>()
            .FirstOrDefaultAsync(x =>
                x.CompanyID == request.CompanyID &&
                x.SiteID == request.SiteID &&
                x.LocationGUID == request.LocationGUID &&
                x.InvoiceNumber == request.InvoiceNumber);

        if (header == null)
            return NotFound("Transaction header not found.");

        var nextLineNumber =
            await _context.Set<POSTransactionsDetail>()
                .Where(x =>
                    x.CompanyID == request.CompanyID &&
                    x.SiteID == request.SiteID &&
                    x.LocationGUID == request.LocationGUID &&
                    x.InvoiceNumber == request.InvoiceNumber)
                .Select(x => (int?)x.LineNumber)
                .MaxAsync() ?? 0;

        nextLineNumber++;

        var qty = request.Quantity ?? 1m;
        var price = request.ItemPrice ?? 0m;
        var tax = request.LineItemTaxAmount ?? 0m;
        var totalPrice = Math.Round(qty * price, 2);

        var detail = new POSTransactionsDetail
        {
            CompanyID = request.CompanyID,
            SiteID = request.SiteID,
            LocationGUID = request.LocationGUID,
            InvoiceNumber = request.InvoiceNumber,
            LineNumber = nextLineNumber,
            ItemID = request.ItemID,
            ItemDescription = request.ItemDescription,
            ItemPrice = request.ItemPrice,
            Quantity = request.Quantity,
            LineItemTaxAmount = request.LineItemTaxAmount,
            TotalPrice = totalPrice,
            TaxableInd = request.TaxableInd,
            FuelPrepayInd = request.FuelPrepayInd,
            FuelFillupInd = request.FuelFillupInd,
            DispenserNumber = request.DispenserNumber,
            PriceOverride = request.PriceOverride,
            NonSalePrice = request.NonSalePrice,
            SpecialInd = request.SpecialInd,
            CategoryCode = request.CategoryCode,
            SafeDropInd = request.SafeDropInd,
            DiscountInd = request.DiscountInd,
            DiscountCode = request.DiscountCode,
            LoyaltyInd = request.LoyaltyInd,
            LoyaltyCode = request.LoyaltyCode
        };

        _context.Set<POSTransactionsDetail>().Add(detail);

        await _context.SaveChangesAsync();
        await RecalculateHeaderAsync(request.CompanyID, request.SiteID, request.LocationGUID, request.InvoiceNumber);

        return Ok(new
        {
            ok = true,
            transactionNumber = request.InvoiceNumber,
            invoiceNumber = request.InvoiceNumber,
            lineNumber = nextLineNumber,
            line = detail
        });
    }

    [HttpPost("payment")]
    public async Task<IActionResult> UpdatePayment([FromBody] UpdatePaymentRequest request)
    {
        var header = await _context.Set<POSTransaction>()
            .FirstOrDefaultAsync(x =>
                x.CompanyID == request.CompanyID &&
                x.SiteID == request.SiteID &&
                x.LocationGUID == request.LocationGUID &&
                x.InvoiceNumber == request.InvoiceNumber);

        if (header == null)
            return NotFound("Transaction header not found.");

        header.AmountPaid = request.AmountPaid;
        header.CashAmount = request.CashAmount;
        header.CheckAmount = request.CheckAmount;
        header.CreditAmount = request.CreditAmount;
        header.CheckNumber = request.CheckNumber;
        header.PaymentType = request.PaymentType;
        header.CardType = request.CardType;
        header.First6 = request.First6;
        header.Last4 = request.Last4;
        header.CardExpirationMM = request.CardExpirationMM;
        header.CardExpirationYY = request.CardExpirationYY;
        header.ReceiptType = request.ReceiptType;
        header.CardTrack2 = request.CardTrack2;
        header.CardResponse = request.CardResponse;

        header.AmountBalance = (header.TotalAmount ?? 0) - (header.AmountPaid ?? 0);

        await _context.SaveChangesAsync();

        return Ok(new
        {
            ok = true,
            transactionNumber = header.InvoiceNumber,
            invoiceNumber = header.InvoiceNumber,
            amountPaid = header.AmountPaid,
            amountBalance = header.AmountBalance
        });
    }

    [HttpPost("{locationGuid:guid}/{invoiceNumber:int}/pending")]
    public async Task<IActionResult> SetPending(Guid locationGuid, int invoiceNumber, [FromQuery] int companyId, [FromQuery] int siteId)
    {
        var header = await _context.Set<POSTransaction>()
            .FirstOrDefaultAsync(x =>
                x.CompanyID == companyId &&
                x.SiteID == siteId &&
                x.LocationGUID == locationGuid &&
                x.InvoiceNumber == invoiceNumber);

        if (header == null)
            return NotFound("Transaction header not found.");

        await RecalculateHeaderAsync(companyId, siteId, locationGuid, invoiceNumber);

        header.OrderStatus = "D";
        await _context.SaveChangesAsync();

        return Ok(new
        {
            ok = true,
            transactionNumber = invoiceNumber,
            invoiceNumber,
            orderStatus = header.OrderStatus,
            totalAmount = header.TotalAmount
        });
    }

    [HttpGet("{locationGuid:guid}/{invoiceNumber:int}")]
    public async Task<IActionResult> GetTransaction(Guid locationGuid, int invoiceNumber, [FromQuery] int companyId, [FromQuery] int siteId)
    {
        var header = await _context.Set<POSTransaction>()
            .FirstOrDefaultAsync(x =>
                x.CompanyID == companyId &&
                x.SiteID == siteId &&
                x.LocationGUID == locationGuid &&
                x.InvoiceNumber == invoiceNumber);

        if (header == null)
            return NotFound("Transaction not found.");

        var lines = await _context.Set<POSTransactionsDetail>()
            .Where(x =>
                x.CompanyID == companyId &&
                x.SiteID == siteId &&
                x.LocationGUID == locationGuid &&
                x.InvoiceNumber == invoiceNumber)
            .OrderBy(x => x.LineNumber)
            .ToListAsync();

        return Ok(new
        {
            ok = true,
            transactionNumber = invoiceNumber,
            invoiceNumber,
            header,
            lines
        });
    }

    private async Task RecalculateHeaderAsync(int companyId, int siteId, Guid locationGuid, int invoiceNumber)
    {
        var header = await _context.Set<POSTransaction>()
            .FirstAsync(x =>
                x.CompanyID == companyId &&
                x.SiteID == siteId &&
                x.LocationGUID == locationGuid &&
                x.InvoiceNumber == invoiceNumber);

        var lines = await _context.Set<POSTransactionsDetail>()
            .Where(x =>
                x.CompanyID == companyId &&
                x.SiteID == siteId &&
                x.LocationGUID == locationGuid &&
                x.InvoiceNumber == invoiceNumber)
            .ToListAsync();

        var subtotal = lines.Sum(x => x.TotalPrice ?? 0m);
        var totalTax = lines.Sum(x => x.LineItemTaxAmount ?? 0m);

        header.Subtotal = subtotal;
        header.SalesTax = totalTax;
        header.TotalTax = totalTax;
        header.TotalAmount = subtotal + totalTax;
        header.AmountBalance = (header.TotalAmount ?? 0) - (header.AmountPaid ?? 0);

        await _context.SaveChangesAsync();
    }
}