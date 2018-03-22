using Hogwarts.Models;
using Hogwarts.Utility;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Hogwarts.Startup))]
namespace Hogwarts
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createRolesandUsers();
        }

        private void createRolesandUsers()
        {
            UserUtility.CreateRole("Admin");
            UserUtility.CreateRole("52th");
            UserUtility.CreateRole("53th");
            UserUtility.CreateRole("54th");
            UserUtility.CreateRole("55th");
            UserUtility.CreateRole("56th");
            UserUtility.CreateRole("57th");
            UserUtility.CreateRole("58th");
            UserUtility.CreateRole("59th");
            UserUtility.CreateRole("60th");

            HogwartsSettingUtility.CreateHogwartsSettingInitialize(HogwartsSettingUtility.NowDisplayRole, "52th");

            UserUtility.CreateUser("manager", "manager", "manager@test.com", "P@ssw0rd", "Admin");
            UserUtility.CreateRole("Sample");
            UserUtility.CreateUser("sample1", "sample1", "sample1@test.com", "P@ssw0rd", "Sample");
            UserUtility.CreateUser("sample2", "sample2", "sample2@test.com", "P@ssw0rd", "Sample");
            UserUtility.CreateUser("sample3", "sample3", "sample3@test.com", "P@ssw0rd", "Sample");
            UserUtility.CreateUser("sample4", "sample4", "sample4@test.com", "P@ssw0rd", "Sample");
            UserUtility.CreateUser("sample5", "sample5", "sample5@test.com", "P@ssw0rd", "Sample");

            

        }
    }
}
