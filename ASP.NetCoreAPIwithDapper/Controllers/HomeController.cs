using ASP.NetCoreAPIwithDapper.Models;
using ASP.NetCoreAPIwithDapper.Services.Interface;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace ASP.NetCoreAPIwithDapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private IDapper _dapper;

        public HomeController(IDapper dapper)
        {
            _dapper = dapper;
        }

        [HttpPost(nameof(Create))]
        public async Task<int> Create(Parameters data)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("Age",data.Age,DbType.Int32);
            dbparams.Add("Name", data.Name,DbType.String);
            var result = await Task.FromResult(_dapper.Insert<int>("[dbo].[Add_User]", dbparams, commandType: CommandType.StoredProcedure));
            return result;
        }
        [HttpGet(nameof(GetByID))]
        public async Task<Parameters> GetByID(int ID)
        {
            var result = await Task.FromResult(_dapper.Get<Parameters>($"Select * from [Users] where Id={ID}", null, CommandType.Text));
            return result;
        }

        [HttpDelete(nameof(Delete))]
        public async Task<int> Delete(int Id)
        {
            var result = await Task.FromResult(_dapper.Execute($"Delete [Users] Where Id = {Id}", null, commandType: CommandType.Text));
            return result;
        }
        [HttpGet(nameof(Count))]
        public Task<int> Count(int num)
        {
            var totalcount = Task.FromResult(_dapper.Get<int>($"select COUNT(*) from [Users] WHERE Age like '%{num}%'", null,
                    commandType: CommandType.Text));
            return totalcount;
        }
        [HttpPatch(nameof(Update))]
        public Task<int> Update(Parameters data)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("Id", data.Id);
            dbPara.Add("Name", data.Name, DbType.String);

            var updateArticle = Task.FromResult(_dapper.Update<int>("[dbo].[Update_User]",
                            dbPara,
                            commandType: CommandType.StoredProcedure));
            return updateArticle;
        }
    }
}
