using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using TransactionMgnt;

namespace GetTransactionController
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetTransactionController : ControllerBase
    {
        public GetTransactionController()
        {
        
        }
        [HttpGet]
        [EnableQuery()]
        public IQueryable<TransactionMgnt.Transaction> Get()
        {
            var db = new DatabaseContext();
            return db.Transaction;
        }
    }
}
