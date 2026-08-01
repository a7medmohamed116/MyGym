using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyGym.Context;
using MyGym.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GymManagement.DAL.Context
{
    // static => helpers,Utility classes
    public static class GymDataSeeding
    {
        public static async Task SeedAsync(GymDbContext dbContext,string seedfolderpath,ILogger logger) 
        {


            try 
            {

                //Get  Data From json File
                //check if table plans has data or not
                if (!await dbContext.Plans.AnyAsync())
                {
                    //seed from json
                    // Generic Method for any file 
                    var plans = LoadDataFromJsonFile<Plan>(seedfolderpath, "plans.json");

                    if (plans.Any())
                    {
                        dbContext.Plans.AddRange(plans);
                        logger.LogInformation($"PLans Seeded With Count {plans.Count}");
                    }
                    //savechangesasync // check for changetracker has done any change in it or no  to avoid do savechange كل شويه
                    if (dbContext.ChangeTracker.HasChanges())
                    {
                        await dbContext.SaveChangesAsync();
                    }
                    else
                    {
                        logger.LogInformation("Plans Already  Seeded");
                    }
                }

            }


            catch(Exception  ex)
            {
                logger.LogInformation(ex,"Seeding Failed");
                throw;

            }
            
            
            

        }

        // Generic Method
        public static List<T> LoadDataFromJsonFile<T>(string FolderPath,string FileName)
        {
            //file path 
            //D:\.NET Core\MVC\Projects\MyGym\MyGym\MyGym\wwwroot\           Files\plans.json
            var filepath = Path.Combine(FolderPath, FileName);
            if (!File.Exists(filepath)) throw new FileNotFoundException("File Date Not Found");
            //read date from json file
            var data = File.ReadAllText(filepath);
            // important option => ignore to case sensitive
            var options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };

            //confert it from json to my list
            return JsonSerializer.Deserialize<List<T>>(data,options)?? []; // {??} if returned null make empty collection  
            

        }
    } // Ilogger => paramter check and print message in the log console

    // in wwwroot add file [Files] and drag drop the json fiels
    // here in DAL at Context file add new context class {gymdataseeding}
    // Add the two methods
    // call [seedasync] in program.cs after build using app.
    //not effecient to create a bulk code in program.cs so we will create method and assign ot to app. how ??? i will steal it see app from what kind and do like him
    // to reduce code in program.cs 
    // in file programextensions 

}
