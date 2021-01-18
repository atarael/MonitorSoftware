using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;

namespace ClientSide
{

    // Set 'RunInstaller' attribute to true.
    [RunInstaller(true)]
    public class MyInstallerClass : Installer
    {
        public MyInstallerClass() : base()
        {
            // Attach the 'Committed' event.
            this.Committed += new InstallEventHandler(MyInstaller_Committed);
            // Attach the 'Committing' event.
            this.Committing += new InstallEventHandler(MyInstaller_Committing);
        }
        // Event handler for 'Committing' event.
        private void MyInstaller_Committing(object sender, InstallEventArgs e)
        {
            Console.WriteLine("");
            Console.WriteLine("Committing Event occurred.");
            Console.WriteLine("");
        }
        // Event handler for 'Committed' event.
        private void MyInstaller_Committed(object sender, InstallEventArgs e)
        {
            Console.WriteLine("");
            Console.WriteLine("Committed Event occurred.");
            Console.WriteLine("");
        }
        // Override the 'Install' method.
        public override void Install(IDictionary savedState)
        {
            base.Install(savedState);
        }
        // Override the 'Commit' method.
        public override void Commit(IDictionary savedState)
        {
            base.Commit(savedState);
        }
        // Override the 'Rollback' method.
        public override void Rollback(IDictionary savedState)
        {
            base.Rollback(savedState);
        }
     /*   public static void Main()
        {
            Console.WriteLine("Usage : installutil.exe Installer.exe ");
        }*/
    }
    
}
