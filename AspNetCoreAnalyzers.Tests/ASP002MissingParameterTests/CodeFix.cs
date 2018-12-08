namespace AspNetCoreAnalyzers.Tests.ASP002MissingParameterTests
{
    using Gu.Roslyn.Asserts;
    using Microsoft.CodeAnalysis.Diagnostics;
    using NUnit.Framework;

    public class CodeFix
    {
        private static readonly DiagnosticAnalyzer Analyzer = new AttributeAnalyzer();
        private static readonly ExpectedDiagnostic ExpectedDiagnostic = ExpectedDiagnostic.Create(ASP002MissingParameter.Descriptor);

        [Test]
        public void WhenNoParameter()
        {
            var code = @"
namespace ValidCode
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    public class OrdersController : Controller
    {
        [HttpGet(""api/orders/{id}"")]
        public async Task<IActionResult> GetOrder↓()
        {
        }
    }
}";
            AnalyzerAssert.Diagnostics(Analyzer, ExpectedDiagnostic, code);
        }

        [TestCase("[FromHeader]")]
        [TestCase("[FromBody]")]
        public void WhenWrongAttribute(string attribute)
        {
            var order = @"
namespace ValidCode
{
    public class Order
    {
        public int Id { get; set; }
    }
}";

            var db = @"
namespace ValidCode
{
    using Microsoft.EntityFrameworkCore;

    public class Db : DbContext
    {
        public DbSet<Order> Orders { get; set; }
    }
}";
            var code = @"
namespace ValidCode
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [ApiController]
    public class OrdersController : Controller
    {
        private readonly Db db;

        public OrdersController(Db db)
        {
            this.db = db;
        }

        [HttpGet(""api/orders/{id}"")]
        public async Task<IActionResult> GetOrder↓([FromHeader]int headerValue)
        {
            var match = await this.db.Orders.FirstOrDefaultAsync(x => x.Id == headerValue);
            if (match == null)
            {
                return this.NotFound();
            }

            return this.Ok(match);
        }
    }
}".AssertReplace("[FromHeader]", attribute);
            AnalyzerAssert.Diagnostics(Analyzer, ExpectedDiagnostic, order, db, code);
        }
    }
}