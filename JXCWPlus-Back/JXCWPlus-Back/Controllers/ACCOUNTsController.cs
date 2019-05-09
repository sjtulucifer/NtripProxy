using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using JXCWPlus_Back.DTOs;
using JXCWPlus_Back.Models;

namespace JXCWPlus_Back.Controllers
{
    // <summary>
    /// 账号Web操作类异步
    /// </summary>
    [RoutePrefix("api/AccountAsy")]
    public class ACCOUNTsController : ApiController
    {
        private JXCWPlus db = new JXCWPlus();

        // Typed lambda expression for Select() method. 
        private static readonly Expression<Func<ACCOUNT, AccountDetail>> AsAccountDetail =
            a => new AccountDetail
            {
                AccountName = a.Account_Name,
                AccountPassword = a.Account_Password,
                AccountRegister = a.Account_Register,
                AccountExpire = a.Account_Expire,
                AccountIsLocked = a.Account_IsLocked
            };

        /// <summary>
        /// 查找所有账号
        /// </summary>
        /// <returns>所有账号列表</returns>
        [HttpGet]
        [Route("GetAccountList")]
        [ResponseType(typeof(List<AccountDetail>))]
        public async Task<IHttpActionResult> GetAccountList()
        {
            List<AccountDetail> accountList = await db.ACCOUNTs.Where(a=>a.isDelete == false).Select(AsAccountDetail).ToListAsync();
            if (accountList == null)
            {
                return NotFound();
            }
            return Json(accountList);
        }

        // <summary>
        /// 通过ID查找账号信息
        /// </summary>
        /// <param name="id">账号ID号</param>
        /// <returns>找到的账号实体</returns>
        [HttpGet]
        [Route("GetAccountByID/{id}")]
        [ResponseType(typeof(AccountDetail))]
        public async Task<IHttpActionResult> GetACCOUNTByID(Guid id)
        {
            AccountDetail accountDetail = await db.ACCOUNTs.Where(a=>a.isDelete == false && a.ID == id)
                                                .Select(AsAccountDetail)
                                                .FirstOrDefaultAsync();
            if (accountDetail == null)
            {
                return NotFound();
            }

            return Json(accountDetail);
        }

        /// <summary>
        /// 账号登录
        /// </summary>
        /// <param name="account">需要登录的账号，必须包含用户名和密码</param>
        /// <returns>可登录的账户信息</returns>
        [HttpPost]
        [Route("AccountLogin")]
        [ResponseType(typeof(AccountDetail))]
        public async Task<IHttpActionResult> AccountLogin(AccountDetail account)
        {
            AccountDetail accountDetail = await db.ACCOUNTs.Where(a => a.isDelete == false && a.Account_Name == account.AccountName)
                                                    .Select(AsAccountDetail)
                                                    .FirstOrDefaultAsync();
            //如果账号未找到或者密码不正确
            if(accountDetail == null || accountDetail.AccountPassword != account.AccountPassword)
            {
                return NotFound();
            }

            return Json(accountDetail);
        }

        // PUT: api/ACCOUNTs/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutACCOUNT(Guid id, ACCOUNT aCCOUNT)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != aCCOUNT.ID)
            {
                return BadRequest();
            }

            db.Entry(aCCOUNT).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ACCOUNTExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/ACCOUNTs
        [ResponseType(typeof(ACCOUNT))]
        public async Task<IHttpActionResult> PostACCOUNT(ACCOUNT aCCOUNT)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ACCOUNTs.Add(aCCOUNT);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ACCOUNTExists(aCCOUNT.ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = aCCOUNT.ID }, aCCOUNT);
        }

        // DELETE: api/ACCOUNTs/5
        [ResponseType(typeof(ACCOUNT))]
        public async Task<IHttpActionResult> DeleteACCOUNT(Guid id)
        {
            ACCOUNT aCCOUNT = await db.ACCOUNTs.FindAsync(id);
            if (aCCOUNT == null)
            {
                return NotFound();
            }

            db.ACCOUNTs.Remove(aCCOUNT);
            await db.SaveChangesAsync();

            return Ok(aCCOUNT);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ACCOUNTExists(Guid id)
        {
            return db.ACCOUNTs.Count(e => e.ID == id) > 0;
        }
    }
}