//--------------------------------------------------------
// Authors: David, Matthew, Levon, Alex, and Derek
// Program: Batty
// Purpose: Handle a table of game controls. 
// Used to check for key presses during gameplay
//--------------------------------------------------------
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Windows.Forms;
using System.Windows.Input;
using System;

namespace Batty_2._0
{
   /// <summary>
   /// Class used for controlling the game controls and handling the controls table
   /// </summary>
   public class Controls
   {
      private static Controls instance;

      public enum ControlType { Left, Right, Up, Down, Pause, Shoot, Exit, Skip, Lives};

      private const int CONTROL_COLUMNS = 3;
      private DataTable keyTable;

      private Dictionary<string, Key[]> keyValues;

      public DataTable ControlsTable { get { return keyTable; } }

      /// <summary>
      /// Constructor: Sets up controls table and default controls
      /// </summary>
      private Controls()
      {
         keyTable = new DataTable("Instructions");
         keyValues = new Dictionary<string, Key[]>();
         SetUpKeyValues();
         SetUpTable();
         SetUpKeyRows();
      }

      /// <summary>
      /// Sets Controls as a singleton class accessed through Instance
      /// </summary>
      public static Controls Instance
      {
         get
         {
            if (instance == null)
               instance = new Controls();
            return instance;
         }
      }

      /// <summary>
      /// Sets up control/key value pairs in map
      /// </summary>
      private void SetUpKeyValues()
      {
         keyValues.Add("Release/Shoot", new Key[2] { Key.Space, Key.Enter });
         keyValues.Add("Move Left", new Key[2] { Key.Left, Key.A });
         keyValues.Add("Move Right", new Key[2] { Key.Right, Key.D });
         keyValues.Add("Move Up", new Key[2] { Key.Up, Key.W });
         keyValues.Add("Move Down", new Key[2] { Key.Down, Key.S });
         keyValues.Add("Exit", new Key[2] { Key.Escape, Key.None });
         keyValues.Add("Pause", new Key[2] { Key.P, Key.None });
         keyValues.Add("Skip Level", new Key[2] { Key.Tab, Key.None });
         keyValues.Add("Infinite Lives", new Key[2] { Key.L, Key.None });
      }

      /// <summary>
      /// Sets up the controls data table
      /// </summary>
      private void SetUpTable()
      {
         keyTable.Columns.Add("Control", typeof(string));
         keyTable.Columns.Add("Key 1", typeof(string));
         keyTable.Columns.Add("Key 2", typeof(string));
         keyTable.PrimaryKey = new DataColumn[] { keyTable.Columns["Control"] };
      }

      /// <summary>
      /// Adds rows to the controls table with data from maps
      /// </summary>
      private void SetUpKeyRows()
      {
         string key;
         for (int i = 0; i < keyValues.Keys.Count; i++)
         {
            key = keyValues.Keys.ElementAt(i);
            if(key != "Skip Level" && key != "Infinite Lives")
               keyTable.Rows.Add(key, keyValues[key][0].ToString(), keyValues[key][1].ToString());
         }

      }

      /// <summary>
      /// sets the values for the default keys
      /// </summary>
      private void DefaultKeys()
      {
         string dictKey;
         DataRow row;
         SetUpKeyValues();
         for (int i = 0; i < keyValues.Keys.Count; i++)
         {
            dictKey = keyValues.Keys.ElementAt(i);
            row = keyTable.Rows.Find(dictKey);
            for (int j = 0; j <= 1; j++)
            {
               row[j + 1] = keyValues[dictKey][j].ToString();
            }
         }
      }

      /// <summary>
      /// Check to see if the key/s associated with given control are pressed
      /// </summary>
      /// <param name="control"></param>
      /// <returns>Returns true if key associated with control is pressed</returns>
      public bool ControlWasPressed(ControlType control)
      {
         try
         {
            if (control == ControlType.Left)
               return Keyboard.IsKeyDown(keyValues["Move Left"][0]) || Keyboard.IsKeyDown(keyValues["Move Left"][1]);
            else if (control == ControlType.Right)
               return Keyboard.IsKeyDown(keyValues["Move Right"][0]) || Keyboard.IsKeyDown(keyValues["Move Right"][1]);
            else if (control == ControlType.Pause)
               return Keyboard.IsKeyDown(keyValues["Pause"][0]) || Keyboard.IsKeyDown(keyValues["Pause"][1]);
            else if (control == ControlType.Shoot)
               return Keyboard.IsKeyDown(keyValues["Release/Shoot"][0]) || Keyboard.IsKeyDown(keyValues["Release/Shoot"][1]);
            else if (control == ControlType.Exit)
               return Keyboard.IsKeyDown(keyValues["Exit"][0]) || Keyboard.IsKeyDown(keyValues["Exit"][1]);
            else if (control == ControlType.Up)
               return Keyboard.IsKeyDown(keyValues["Move Up"][0]) || Keyboard.IsKeyDown(keyValues["Move Up"][1]);
            else if (control == ControlType.Down)
               return Keyboard.IsKeyDown(keyValues["Move Down"][0]) || Keyboard.IsKeyDown(keyValues["Move Down"][1]);
            else if (control == ControlType.Skip)
               return Keyboard.IsKeyDown(keyValues["Skip Level"][0]) || Keyboard.IsKeyDown(keyValues["Skip Level"][1]);
            else if (control == ControlType.Lives)
               return Keyboard.IsKeyDown(keyValues["Infinite Lives"][0]) || Keyboard.IsKeyDown(keyValues["Infinite Lives"][1]);
            else
               return false;
         }
         catch (Exception)
         {
            return false;
         }
      }

      /// <summary>
      /// Change current control key at given index to given key
      /// </summary>
      /// <param name="control">Control name</param>
      /// <param name="keyCode">Key code recieved from keyPress event</param>
      /// <param name="index">Index of first or second key</param>
      public void ChangeKey(string control, Keys keyCode, int index)
      {
         Key inKey = KeyInterop.KeyFromVirtualKey((int)keyCode);

         DataRow row;

         CheckKeyDuplicate(inKey);

         keyValues[control][index - 1] = inKey;
         row = keyTable.Rows.Find(control);
         row[index] = keyCode.ToString();
      }

      /// <summary>
      /// Checks if given key is already in the key dictionary
      /// If it is, set it to "None" key state
      /// </summary>
      /// <param name="inputKey"></param>
      private void CheckKeyDuplicate(Key inputKey)
      {
         DataRow row;
         string dictKey;
         for (int i = 0; i < keyValues.Keys.Count; i++)
         {
            dictKey = keyValues.Keys.ElementAt(i);
            for (int j = 0; j <= 1; j++)
            {
               if (keyValues[dictKey][j] == inputKey)
               {
                  keyValues[dictKey][j] = Key.None;
                  row = keyTable.Rows.Find(dictKey);
                  row[j + 1] = Keys.None;
                  return;
               }
            }
         }
      }
   }
}
