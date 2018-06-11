using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Octokit;

namespace BMILib
{
    static class Utils
    {
        public static Uri ghe = new Uri("http://bmi.battletechgame.btmux.com:8086/");
        public static GitHubClient ghClient = new GitHubClient(new ProductHeaderValue("Battletech-Mod-Installer"), ghe);
    }
}
