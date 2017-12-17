
using DuPont.Interface;
using DuPont.Models.Models;
using DuPont.Utility;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Repository
{
    public class OperatorInfoVerificationRepository : BaseRepository<T_MACHINERY_OPERATOR_VERIFICATION_INFO>, IOperatorInfoVerifciationRepository
    {
        /// <summary>
        /// E田农机手取消订单
        /// </summary>
        /// <param name="fmodel"></param>
        /// <param name="id"></param>
        /// <param name="OperatorUserId"></param>
        /// <returns></returns>
        public async Task<int> UpdateDEMAND(T_FARMER_PUBLISHED_DEMAND fmodel, long id, string OperatorUserId)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                using (var dbTransaction = dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        if (fmodel.Id == id)
                        {
                            fmodel.ModifiedTime = DateTime.Now;
                            var dbEntry = dbContext.Entry<T_FARMER_PUBLISHED_DEMAND>(fmodel);
                            dbEntry.State = EntityState.Modified;                           
                            var operatoruser = dbContext.T_USER.SingleOrDefault(x => x.WeatherCity == OperatorUserId);
                            long operatoruserid = 0;
                            if (operatoruser == null)
                            {
                                operatoruserid = dbContext.T_USER.SingleOrDefault(x => x.Id.ToString() == OperatorUserId).Id;
                            }
                            else {
                                operatoruserid = operatoruser.Id;
                            }
                            var responsemodel = dbContext.T_FARMER_DEMAND_RESPONSE_RELATION.SingleOrDefault(x => x.DemandId == id && x.UserId == operatoruserid);
                            dbContext.T_FARMER_DEMAND_RESPONSE_RELATION.Remove(responsemodel);
                        }
                       var r =await dbContext.SaveChangesAsync();//要使用异步保存，必须使用await
                        dbTransaction.Commit();
                        return r;
                    }
                    catch (Exception ex){                        
                        dbTransaction.Rollback();
                        return  0;
                    }
                }                
            }
        }
    }
}
