using Batty_2._0;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using System.Windows.Forms;
using System.Windows.Input;
using System.Threading;

namespace Batty_2._0_Test
{
   [TestClass]
   public class ControlsTest
   {
      [TestMethod]
      public void Controls_Instance_NotNull()
      {
         Assert.IsNotNull(Batty_2._0.Controls.Instance);
      }

      [TestMethod]
      public void Controls_DataTable_NotNull()
      {
         DataTable dt;
         dt = Batty_2._0.Controls.Instance.ControlsTable;
         Assert.IsNotNull(dt);
      }

      [TestMethod]
      public void Controls_SetUpTable_Valid()
      {
         DataTable dt;
         dt = Batty_2._0.Controls.Instance.ControlsTable;
         Assert.IsTrue(dt.Columns[0].ColumnName.Equals("Control"));
         Assert.IsTrue(dt.Columns[1].ColumnName.Equals("Key 1"));
         Assert.IsTrue(dt.Columns[2].ColumnName.Equals("Key 2"));
      }

      [TestMethod]
      public void Controls_SetUpRows_Valid()
      {
         DataTable dt;
         dt = Batty_2._0.Controls.Instance.ControlsTable;
         Assert.IsTrue(dt.Rows.Count > 0);
      }

      //[TestMethod]
      //public void Controls_CheckControls_Valid()
      //{
         
      //   SendKeys.SendWait("{LEFT}");
      //   Assert.IsTrue(Controls.Instance.ControlWasPressed(Controls.ControlType.Left));
      //   SendKeys.SendWait("{RIGHT}");
      //   Assert.IsTrue(Controls.Instance.ControlWasPressed(Controls.ControlType.Right));
      //   SendKeys.SendWait("{UP}");
      //   Assert.IsTrue(Controls.Instance.ControlWasPressed(Controls.ControlType.Up));
      //   SendKeys.SendWait("{DOWN}");
      //   Assert.IsTrue(Controls.Instance.ControlWasPressed(Controls.ControlType.Down));
      //   SendKeys.SendWait("{P}");
      //   Assert.IsTrue(Controls.Instance.ControlWasPressed(Controls.ControlType.Pause));
      //   SendKeys.SendWait("{SPACE}");
      //   Assert.IsTrue(Controls.Instance.ControlWasPressed(Controls.ControlType.Shoot));
      //   SendKeys.SendWait("{ESC}");
      //   Assert.IsTrue(Controls.Instance.ControlWasPressed(Controls.ControlType.Exit));
      //}

      [TestMethod]
      public void Controls_ChangeControls_Processed()
      {
            int changeIndex = 1;
            Controls.Instance.ChangeKey("Move Left", Keys.R, changeIndex);

            DataTable dt;
            DataRow tempRow = null;
            dt = Controls.Instance.ControlsTable;

            foreach (DataRow row in dt.Rows)
            {
                if (row[0].Equals("Move Left"))
                {
                    tempRow = row;
                    break;
                }
            }

            Assert.IsTrue(tempRow[changeIndex].Equals("R"));
      }

      [TestMethod]
      public void Controls_ChangeControlsDuplicate_Processed()
      {
            int changeIndex = 1;
            Controls.Instance.ChangeKey("Move Right", Keys.P, changeIndex);
         DataTable dt;
         DataRow tempRow = null;
         dt = Controls.Instance.ControlsTable;

         foreach (DataRow row in dt.Rows)
         {
            if (row[0].Equals("Pause"))
            {
               tempRow = row;
               break;
            }  
         }
         Assert.IsTrue(tempRow[changeIndex].Equals("None"));

      }
   }
}
