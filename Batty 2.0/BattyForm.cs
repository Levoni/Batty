using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

//using System.Windows.Input;

namespace Batty_2._0
{
   public partial class GameForm : Form
   {
      private int introBattyFontSize;
      private const int MAX_INTROFONT = 24;
      private const int MOVE_STEP = 15;
      private const int INTRO_TIMER_INTERVAL = 100;
      private const int GAME_TIMER_INTERVAL = 15;
      private const int PAUSE_TIME = 500;
      private const int ITEM_SIZE_SPLIT = 2; // Used to calculate item size split in half
      private const int QUARTER_POSITION_SPLIT = 4;
      private const int NAME_COUNT = 5;
      private const int NAME_SPACE_COUNT = 2; // Used to calculate the total spacing between each name label
      private const int EDITOR_ROW_COUNT = 10;
      private const int EDITOR_COL_COUNT = 12;

      private bool srcCodeLbl2Active = false, srcCodeLblSwitched = false;

      private const bool SKIP_INTRO = false; // Used to skip intro for testing purposes

      private bool introBattyEffect;
      private bool introNameEffect;
      private bool introMadeEffect;

      private readonly Color[] colorArray = { Color.Red, Color.Yellow,
         Color.Pink, Color.Blue, Color.Orange, Color.Green, Color.Cyan, Color.Fuchsia, Color.Lime, Color.White};
      private int colorIndexMM;
      private int colorIndexHS;
      private int colorBallIndex;

      private readonly Color[] paddleColors = { Color.Transparent, Color.Red, Color.Yellow,
         Color.Pink, Color.Blue, Color.Orange, Color.Green, Color.Cyan, Color.Lime, Color.White};
      private int colorPaddleIndex;
      private int bgMusicIndex;

      private Label[] menuItems;
      private int menuItemIndex;

      private EditorController editorManager;

      private GameManager gameManager;

      private int editorLivesCounter;
      private const int MAX_LIVES = 3;

      private int optLivesClickCount, initLives;

      private HighScoreTable hScoreTable;

      private SolidBrush brush;
      private Pen pen;

      private Stack<Panel> panelStack;

      private bool keyBeingHandled;

      private static bool levelEnd;

      private bool gameRunning;


      // FPS Testing feature
      System.Diagnostics.Stopwatch st = new System.Diagnostics.Stopwatch();
      float FPS = 0;
      int n = 0;
      bool FPSDebug = true;
      float elapsed;

      /// <summary>
      /// Initialize game form
      /// </summary>
      public GameForm()
      {
         this.WindowState = FormWindowState.Maximized;
         InitializeComponent();
         HidePanels();
         typeof(Panel).InvokeMember("DoubleBuffered",
               BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
               null, pnlLevel, new object[] { true });
         typeof(Panel).InvokeMember("DoubleBuffered",
               BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
               null, pnlSrcCode, new object[] { true });
         initLives = MAX_LIVES;
      }

      /// <summary>
      /// Set up all panels and variables when form loads
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void GameForm_Load(object sender, EventArgs e)
      {
         SetUpVars();
         SetUpPanels();
         SetUpGame();
         if (SKIP_INTRO)
         {
            //PlaySound(bgMusicPath[bgMusicIndex]);
            SwitchToMenu();
         }
         else
            DoIntroSequence();
      }

      /// <summary>
      /// Hides all panels
      /// </summary>
      private void HidePanels()
      {
         foreach (Control ctrl in this.Controls)
         {
            if (ctrl is Panel)
               ctrl.Visible = false;
         }
      }

      /// <summary>
      /// Sets initail values for highscore table on the highscore panel
      /// </summary>
      private void SetUpHighScoreTable()
      {  
         dgvHighScore.DataSource = hScoreTable.HighScores;
         if(dgvHighScore.Rows.Count == 0)
            dgvHighScore.RowTemplate.Height = (dgvHighScore.Height - dgvHighScore.ColumnHeadersHeight) / 1;
         else
            dgvHighScore.RowTemplate.Height = (dgvHighScore.Height - dgvHighScore.ColumnHeadersHeight) / (dgvHighScore.Rows.Count + 1);
         dgvHighScore.AdvancedCellBorderStyle.All = DataGridViewAdvancedCellBorderStyle.None;
         //dgvHighScore.Rows[2].Cells[0].Selected = true;
         dgvHighScore.RowsDefaultCellStyle.BackColor = pnlHighScore.BackColor;
         dgvHighScore.RowsDefaultCellStyle.ForeColor = Color.Red;
         dgvHighScore.ColumnHeadersDefaultCellStyle.BackColor = pnlHighScore.BackColor;
         dgvHighScore.ColumnHeadersDefaultCellStyle.BackColor = dgvHighScore.RowsDefaultCellStyle.ForeColor;
      }

      /// <summary>
      /// Sets initial valuses for variables use by the form
      /// </summary>
      private void SetUpVars()
      {
         timerIntro.Interval = INTRO_TIMER_INTERVAL;
         timerGame.Interval = GAME_TIMER_INTERVAL;
         introBattyFontSize = 1;
         introBattyEffect = false;
         introNameEffect = false;
         introMadeEffect = false;
         gameRunning = false;
         colorIndexMM = 0;
         colorIndexHS = 0;
         colorBallIndex = 0;
         colorPaddleIndex = 0;
         bgMusicIndex = 0;
         menuItemIndex = -1;
         levelEnd = false;

         menuItems = new Label[] { lblStart, lblHighScore, lblInstructions, lblEditor, lblOptions, lblExit };

         optLivesClickCount = 2;
         editorLivesCounter = -1;

         keyBeingHandled = false;

         hScoreTable = new HighScoreTable();
         brush = new SolidBrush(Color.Blue);
         pen = new Pen(Color.Black, Height / 1000 + 1);
         editorManager = new EditorController(GameManager.NUM_ROWS, GameManager.NUM_COLS);

         panelStack = new Stack<Panel>();

         //SetUpBGMusic();
         SetUpEventHandlers();
      }

      /// <summary>
      /// General method that sets up all dynamically assigned event handlers
      /// </summary>
      private void SetUpEventHandlers()
      {
         SetUpOptionsEventHandlers();
         SetUpBackButtons();
         SetUpMenuEventHandlers();
      }

      /// <summary>
      /// Set up options event handlers
      /// </summary>
      private void SetUpOptionsEventHandlers()
      {
         lblSound.Click += ToggleSound;
         lblSoundOpt.Click += ToggleSound;
         lblBallColor.Click += ChangeBallColor;
         picBallColor.Click += ChangeBallColor;

         lblPaddleColor.Click += ChangePaddleColor;
         picPaddleColor.Click += ChangePaddleColor;
      }

      /// <summary>
      /// Assign MouseEnter and MouseLeave event handlers to all menu buttons (labels)
      /// </summary>
      private void SetUpMenuEventHandlers()
      {
         foreach (Control pnlItem in pnlMenu.Controls)
         {
            if (pnlItem is Label)
            {
               if (pnlItem.Text != "Batty")
               {
                  pnlItem.MouseEnter += HandleLabelHover;
                  pnlItem.MouseLeave += HandleLabelExit;
               }
                  
            }
         }
      }

      /// <summary>
      /// initializes the gameManager
      /// </summary>
      private void SetUpGame()
      {
         gameManager = new GameManager(initLives, 0, pnlLevel.Width, pnlLevel.Height, lblScore.Width, pnlLevel.Height / 4, paddleColors[colorPaddleIndex], colorArray[colorBallIndex]);
      }

      /// <summary>
      /// initializes values used by the editor panel
      /// </summary>
      private void SetUpEditor()
      {
         pnlEditor.Size = new Size(Width, Height);
         pnlEditor.Top = 0;
         pnlEditor.Top = Left;
         pnlEditorScreen.Size = new Size(pnlEditor.Width * 9 / 10, pnlEditor.Height * 8 / 10);
         pnlEditorScreen.Top = pnlEditor.Height / 100;
         pnlEditorScreen.Left = pnlEditor.Width / 10;

         int screenSpacingWidth = pnlEditorScreen.Width * 5 / 100;
         int screenSpacingHeight = screenSpacingWidth / 4;

         
         tblEnemyPos.Height = pnlEditorScreen.Height * 7 / 100;

         tblBlockPos.Width = pnlEditorScreen.Width * 9 / 10;
         tblEnemyPos.Width = tblBlockPos.Width;

         tblBlockPos.Height = tblEnemyPos.Height * 5;
         tblBlockPos.Left = screenSpacingWidth;
         tblEnemyPos.Left = tblBlockPos.Left;
         tblEnemyPos.Top = screenSpacingHeight;
         tblBlockPos.Top = tblEnemyPos.Top + screenSpacingHeight + tblEnemyPos.Height;

         grpEditorColor.Top = pnlEditorScreen.Top + pnlEditorScreen.Height + screenSpacingHeight;
         grpEditorLives.Top = grpEditorColor.Top;

         grpEditorColor.Left = pnlEditorScreen.Left;
         grpEditorLives.Left = tblBlockPos.Left + tblBlockPos.Width - grpEditorLives.Width;

         picPaddle.Top = pnlEditorScreen.Height * 9 / 10;
         picPaddle.Left = pnlEditorScreen.Width / 2 - picPaddle.Width / 2;

         picEdtHeart2.Visible = false;
         picEdtHeart3.Visible = false;

         SetUpEditorTables();
         SetUpEditorButtons();
      }

      /// <summary>
      /// Dynamically assign event handlers to all color buttons in the Editor
      /// </summary>
      private void SetUpEditorButtons()
      {
         foreach (Control item in grpEditorColor.Controls)
         {
            if (item is Button)
               item.Click += SetEditorBlockColor;
         }
      }

      /// <summary>
      /// sets up on-click call for all back buttons
      /// </summary>
      private void SetUpBackButtons()
      {
         foreach (Control ctrl in this.Controls)
         {
            foreach (Control pnlItem in ctrl.Controls)
            {
               if (pnlItem is Label)
               {
                  if (pnlItem.Text == "Back")
                  {
                     pnlItem.Click += BackButtonClick;
                     pnlItem.MouseEnter += HandleLabelHover;
                     pnlItem.MouseLeave += HandleLabelExit;
                  }
               }
            }
         }
      }

      /// <summary>
      /// Initialves teh table used by the editor panel
      /// </summary>
      private void SetUpEditorTables()
      {
         tblBlockPos.RowTemplate.Height = (tblBlockPos.Height) / (GameManager.NUM_ROWS - 1);
         tblEnemyPos.RowTemplate.Height = (tblEnemyPos.Height);

         for (int i = 0; i < GameManager.NUM_COLS; i++)
            tblEnemyPos.Columns.Add("Enemy Spot " + i.ToString(), "");
         tblEnemyPos.Rows.Add();

         for (int i = 0; i < GameManager.NUM_COLS; i++)
            tblBlockPos.Columns.Add("Block Spot " + i.ToString(), "");

         for (int i = 0; i < GameManager.NUM_ROWS - 1; i++)
            tblBlockPos.Rows.Add();

         tblBlockPos.ColumnHeadersVisible = false;
         tblEnemyPos.ColumnHeadersVisible = false;

         tblBlockPos.Rows[5].Cells[5].Selected = true;
         tblEnemyPos.Rows[0].Cells[1].Selected = true;

         tblBlockPos.ClearSelection();
         tblEnemyPos.ClearSelection();


      }

      /// <summary>
      /// Initializes the values used to display the options panel
      /// </summary>
      private void SetUpOptions()
      {
         pnlOptions.Size = new Size(this.Width / ITEM_SIZE_SPLIT, this.Height);
         pnlOptions.Left = this.Width / QUARTER_POSITION_SPLIT;
         pnlOptions.Top = 0;

         lblOptionsTitle.Top = lblOptionsTitle.Size.Height / ITEM_SIZE_SPLIT;
         lblOptionsTitle.Left = (pnlOptions.Size.Width / ITEM_SIZE_SPLIT) -
            (lblTitle.Size.Width / ITEM_SIZE_SPLIT);

         lblPaddleLength.Left = (pnlOptions.Size.Width / ITEM_SIZE_SPLIT) - 
            (lblPaddleLength.Size.Width * 3 / 2);
         int lblLeftPos = lblPaddleLength.Left;
         int optItemLeft = lblLeftPos + lblPaddleLength.Width + 5;

         foreach (Control ctrl in pnlOptions.Controls)
         {
            if(ctrl is Label)
            {
               if (ctrl.Name == "lblSoundOpt")
                  ctrl.Left = optItemLeft;
               else if (ctrl.Name != "lblOptionsTitle")
                  ctrl.Left = lblLeftPos;
            }
            else
               ctrl.Left = optItemLeft; 
         }

         picOptionLives2.Left = picOptionLives1.Left + picOptionLives1.Width + picOptionLives1.Width / 10;
         picOptionLives3.Left = picOptionLives2.Left + picOptionLives2.Width + picOptionLives2.Width / 10;

         int spacing = lblPaddleLength.Size.Height + (lblPaddleLength.Size.Height);

         lblPaddleLength.Top = lblOptionsTitle.Top + lblOptionsTitle.Size.Height +
            (lblTitle.Size.Height / 2);
         lblPaddleColor.Top = lblPaddleLength.Top + spacing;
         lblBallColor.Top = lblPaddleColor.Top + spacing;
         lblBallSpeed.Top = lblBallColor.Top + spacing;
         lblOptionLives.Top = lblBallSpeed.Top + spacing;
         lblSound.Top = lblOptionLives.Top + spacing;

         trackPaddleSize.Top = lblPaddleLength.Top;
         picPaddleColor.Top = lblPaddleColor.Top;
         picBallColor.Top = lblBallColor.Top;
         trackBallSpeed.Top = lblBallSpeed.Top;
         picOptionLives1.Top = lblOptionLives.Top;
         picOptionLives2.Top = picOptionLives1.Top;
         picOptionLives3.Top = picOptionLives1.Top;
         lblSoundOpt.Top = lblSound.Top;
      }

      /// <summary>
      /// initialives the values used to display the table on the instructions panel
      /// </summary>
      private void SetUpInstrTable()
      {
         dgvInstructions.DataSource = Batty_2._0.Controls.Instance.ControlsTable;

         dgvInstructions.ClearSelection();

         dgvInstructions.ReadOnly = true;
         dgvInstructions.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
         dgvInstructions.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
         dgvInstructions.RowHeadersVisible = false;
         dgvInstructions.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

         dgvInstructions.Columns[0].ReadOnly = true;
      }

      /// <summary>
      /// Initializes the values use to display the high score panel
      /// </summary>
      private void SetUpHScorePanel()
      {
         pnlHighScore.Size = new Size(this.Width / ITEM_SIZE_SPLIT, this.Height);
         pnlHighScore.Left = this.Width / QUARTER_POSITION_SPLIT;
         pnlHighScore.Top = 0;

         lblHScoreTitle.Top = lblHScoreTitle.Size.Height / ITEM_SIZE_SPLIT;
         lblHScoreTitle.Left = (pnlHighScore.Size.Width / ITEM_SIZE_SPLIT) -
            (lblHScoreTitle.Size.Width / ITEM_SIZE_SPLIT);

         int tableWidth = Convert.ToInt32(pnlHighScore.Size.Width * .8);
         int tableHeight = Convert.ToInt32(pnlHighScore.Size.Height * .7);
         dgvHighScore.Size = new Size(tableWidth, tableHeight);

         dgvHighScore.Left = (pnlHighScore.Size.Width - dgvHighScore.Size.Width) / ITEM_SIZE_SPLIT;
         dgvHighScore.Top = lblHScoreTitle.Top + lblHScoreTitle.Top + lblHScoreTitle.Size.Height;

         lblBackHScore.Left = dgvHighScore.Left;
         lblBackHScore.Top = dgvHighScore.Top + dgvHighScore.Size.Height + lblBackHScore.Size.Height;

         SetUpHighScoreTable();
      }

      /// <summary>
      /// Initializes the values used to display the Postgame panel
      /// </summary>
      private void SetUpPostGamePanel()
      {
         pnlEndLevel.Size = new Size(Width, Height);
         pnlEndLevel.Left = 0;
         pnlEndLevel.Top = 0;

         lblEndStatus.Left = pnlEndLevel.Width / ITEM_SIZE_SPLIT - lblEndStatus.Width / ITEM_SIZE_SPLIT;
         lblEndLevel.Left = pnlEndLevel.Width / ITEM_SIZE_SPLIT - lblEndLevel.Width;
         lblEndScore.Left = lblEndLevel.Left;
         lblEndTime.Left = lblEndLevel.Left;

         lblEndStatus.Top = pnlEndLevel.Height / 2 - lblEndStatus.Height * 5;
         lblEndLevel.Top = lblEndStatus.Top + lblEndStatus.Height + lblEndLevel.Height;
         lblEndScore.Top = lblEndLevel.Top + lblEndLevel.Height + lblEndScore.Height;
         lblEndTime.Top = lblEndScore.Top + lblEndScore.Height + lblEndTime.Height;

         lblEndContinue.Left = pnlEndLevel.Width / ITEM_SIZE_SPLIT - lblEndContinue.Width / ITEM_SIZE_SPLIT;
         lblEndContinue.Top = pnlEndLevel.Height - lblEndContinue.Height * 2;
      }

      /// <summary>
      /// Initializes the values use to display the main game panel
      /// </summary>
      private void SetUpLevelPanel()
      {
         pnlLevel.Size = new Size(Width, Height);
         pnlLevel.Top = 0;
         pnlLevel.Left = 0;

         lblLevel.Left = pnlLevel.Left + lblLevel.Width / 4;
         lblScore.Left = lblLevel.Left;
         lblLives.Left = pnlLevel.Width - lblLives.Width * 3;
         lblTime.Left = lblLives.Left;

         picLives1.Top = lblLives.Top;
         picLives2.Top = picLives1.Top;
         picLives3.Top = picLives1.Top;

         picLives1.Left = lblLives.Left + lblLives.Width + picLives1.Width / 5;
         picLives2.Left = picLives1.Left + picLives1.Width + picLives2.Width / 5;
         picLives3.Left = picLives2.Left + picLives2.Width + picLives3.Width / 5;

         lblPaused.Top = pnlLevel.Height / 2 - lblPaused.Height / 2;
         lblPaused.Left = pnlLevel.Width / 2 - lblPaused.Width / 2;
         lblPaused.Visible = false;
      }

      /// <summary>
      /// Displays the intro sequence 
      /// </summary>
      private void DoIntroSequence()
      {
         System.Threading.Thread.Sleep(PAUSE_TIME); // Wait for game to start
         timerIntro.Enabled = true;
         pnlIntro.Visible = true;
         lblIntroBatty.Visible = true;
         introBattyEffect = true;
      }

      /// <summary>
      /// Initializes values used to display all panels 
      /// calls the setup functions for each panel
      /// </summary>
      private void SetUpPanels()
      {
         pnlMenu.Visible = false;
         pnlInstructions.Visible = false;

         pnlMenu.Size = new Size(this.Width / ITEM_SIZE_SPLIT, this.Height);
         pnlMenu.Left = this.Width / QUARTER_POSITION_SPLIT;
         pnlMenu.Top = 0;

         pnlIntro.Size = new Size(this.Width, this.Height / ITEM_SIZE_SPLIT);
         pnlIntro.Left = 0;
         pnlIntro.Top = this.Height / QUARTER_POSITION_SPLIT;

         pnlInstructions.Size = new Size(this.Width / ITEM_SIZE_SPLIT, this.Height);
         pnlInstructions.Left = this.Width / QUARTER_POSITION_SPLIT;
         pnlInstructions.Top = 0;

         SetUpIntroPanel();
         SetUpInstrPanel();
         SetUpMenu();
         SetUpEditor();
         SetUpLevelPanel();
         SetUpHScorePanel();
         SetUpOptions();
         SetUpPostGamePanel();
         SetUpInputHScorePanel();
         SetUpSrcCodePanel();
      }

      /// <summary>
      /// Aninalts the names in the intro sequence 
      /// </summary>
      private void MoveIntroNameLabels()
      {
         lblDan.Left = pnlIntro.Size.Width;
         lblAlex.Left = pnlIntro.Size.Width;
         lblDerek.Left = pnlIntro.Size.Width;
         lblLevon.Left = 0 - lblLevon.Size.Width;
         lblMatthew.Left = 0 - lblMatthew.Size.Width;

         int spacing = (pnlIntro.Size.Height - (lblDan.Size.Height * NAME_COUNT)) /
            (NAME_COUNT + NAME_SPACE_COUNT);

         lblDan.Top = spacing;
         spacing += lblDan.Size.Height;

         lblLevon.Top = lblDan.Top + spacing;
         lblDerek.Top = lblLevon.Top + spacing;
         lblMatthew.Top = lblDerek.Top + spacing;
         lblAlex.Top = lblMatthew.Top + spacing;
      }

      /// <summary>
      /// Initialives the valuse used to display the intro panel
      /// </summary>
      private void SetUpIntroPanel()
      {
         MoveIntroNameLabels();

         lblMadeBy.Left = (pnlIntro.Size.Width / ITEM_SIZE_SPLIT) - (lblIntroBatty.Size.Width / ITEM_SIZE_SPLIT);
         lblMadeBy.Top = 0 - lblMadeBy.Size.Height;

         lblIntroBatty.Visible = false;
         lblIntroBatty.Font = new Font(lblIntroBatty.Font.FontFamily, introBattyFontSize, FontStyle.Regular);

         lblIntroBatty.Left = (pnlIntro.Size.Width / ITEM_SIZE_SPLIT) - (lblIntroBatty.Size.Width / ITEM_SIZE_SPLIT);
         lblIntroBatty.Top = (pnlIntro.Size.Height / ITEM_SIZE_SPLIT) - (lblIntroBatty.Size.Height / ITEM_SIZE_SPLIT);

      }

      /// <summary>
      /// Set up label positions in the main menu panel
      /// </summary>
      private void SetUpMenu()
      {
         // Set menu title in the center top of the panel
         lblTitle.Top = lblTitle.Size.Height / ITEM_SIZE_SPLIT;
         lblTitle.Left = (pnlMenu.Size.Width / ITEM_SIZE_SPLIT) -
            (lblTitle.Size.Width / ITEM_SIZE_SPLIT);

         // Set all menu items in the middle of the panel
         lblStart.Left = (pnlMenu.Size.Width / ITEM_SIZE_SPLIT) -
            (lblStart.Size.Width / ITEM_SIZE_SPLIT);
         lblOptions.Left = (pnlMenu.Size.Width / ITEM_SIZE_SPLIT) -
            (lblOptions.Size.Width / ITEM_SIZE_SPLIT);
         lblInstructions.Left = (pnlMenu.Size.Width / ITEM_SIZE_SPLIT) -
            (lblInstructions.Size.Width / ITEM_SIZE_SPLIT);
         lblExit.Left = (pnlMenu.Size.Width / ITEM_SIZE_SPLIT) -
            (lblExit.Size.Width / ITEM_SIZE_SPLIT);
         lblEditor.Left = (pnlMenu.Size.Width / ITEM_SIZE_SPLIT) -
            (lblEditor.Size.Width / ITEM_SIZE_SPLIT);
         lblHighScore.Left = (pnlMenu.Size.Width / ITEM_SIZE_SPLIT) -
            (lblHighScore.Size.Width / ITEM_SIZE_SPLIT);

         int spacing = lblStart.Size.Height + (lblStart.Size.Height * ITEM_SIZE_SPLIT);

         lblStart.Top = lblTitle.Top + lblTitle.Size.Height +
            (lblTitle.Size.Height);
         lblHighScore.Top = lblStart.Top + spacing;
         lblInstructions.Top = lblHighScore.Top + spacing;
         lblEditor.Top = lblInstructions.Top + spacing;
         lblOptions.Top = lblEditor.Top + spacing;
         lblExit.Top = lblOptions.Top + spacing;

         lblRestart.Top = lblStart.Top;
         lblRestart.Left = (pnlMenu.Size.Width / ITEM_SIZE_SPLIT) -
            (lblRestart.Width / ITEM_SIZE_SPLIT);
      }

      /// <summary>
      /// Set up label positions in the Instructions panel
      /// </summary>
      private void SetUpInstrPanel()
      {
         SetUpInstrTable();

         lblInstrTitle.Top = lblInstrTitle.Size.Height / ITEM_SIZE_SPLIT;
         lblInstrTitle.Left = (pnlInstructions.Size.Width / ITEM_SIZE_SPLIT) -
            (lblInstrTitle.Size.Width / ITEM_SIZE_SPLIT);

         int tableWidth = Convert.ToInt32(pnlInstructions.Size.Width * .8);
         int tableHeight = Convert.ToInt32(pnlInstructions.Size.Height * .5);
         dgvInstructions.Size = new Size(tableWidth, tableHeight);

         txtDirections.AutoSize = false;
         txtDirections.Width = Convert.ToInt32(pnlInstructions.Size.Width * .8);
         txtDirections.Height = Convert.ToInt32(pnlInstructions.Size.Height * .25);
         txtDirections.Left = (pnlInstructions.Size.Width - txtDirections.Size.Width) / ITEM_SIZE_SPLIT;
         txtDirections.Top = lblInstrTitle.Top + lblInstrTitle.Top + lblInstrTitle.Size.Height;

         dgvInstructions.Left = (pnlInstructions.Size.Width - dgvInstructions.Size.Width) / ITEM_SIZE_SPLIT;
         dgvInstructions.Top = lblInstrTitle.Top + lblInstrTitle.Top + lblInstrTitle.Size.Height + txtDirections.Size.Height; //here

         lblBack.Left = dgvInstructions.Left;
         lblBack.Top = dgvInstructions.Top + dgvInstructions.Size.Height + lblBack.Size.Height;
      }

      /// <summary>
      /// Set up the end game high score screen
      /// </summary>
      private void SetUpInputHScorePanel()
      {
         pnlEndHScore.Size = new Size(this.Width, this.Height / ITEM_SIZE_SPLIT);
         pnlEndHScore.Left = 0;
         pnlEndHScore.Top = this.Height / QUARTER_POSITION_SPLIT;

         lblEndHScoreTitle.Top = pnlEndHScore.Height / ITEM_SIZE_SPLIT - lblEndHScoreTitle.Height * ITEM_SIZE_SPLIT;
         lblEndHScoreTitle.Left = pnlEndHScore.Width / ITEM_SIZE_SPLIT - lblEndHScoreTitle.Width / ITEM_SIZE_SPLIT;

         lblHScoreScore.Top = lblEndHScoreTitle.Top + lblEndHScoreTitle.Height + lblHScoreScore.Height;
         lblHScoreScore.Left = pnlEndHScore.Width / ITEM_SIZE_SPLIT - lblHScoreScore.Width;

         lblName.Top = lblHScoreScore.Top + lblHScoreScore.Height + lblName.Height;
         lblName.Left = pnlEndHScore.Width / ITEM_SIZE_SPLIT - lblName.Width / ITEM_SIZE_SPLIT;

         txtName.Top = lblName.Top + lblName.Height + txtName.Height;
         txtName.Left = pnlEndHScore.Width / ITEM_SIZE_SPLIT - txtName.Width / ITEM_SIZE_SPLIT;
      }

      /// <summary>
      /// Initializes the values used to display the source code panel 
      /// </summary>
      private void SetUpSrcCodePanel()
      {
         pnlSrcCode.Size = new Size(Width, Height);
         pnlSrcCode.Top = 0;
         pnlSrcCode.Left = 0;

         lblSrcCode.Left = 5;
         lblSrcCode.Top = pnlSrcCode.Height;
         lblSrcCode2.Left = 5;
         lblSrcCode2.Top = pnlSrcCode.Height;
      }

      /// <summary>
      /// Every tick of the intro timer move intro objects on the screen
      /// dependent on the current boolean effect that is true
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void timerIntro_Tick(object sender, EventArgs e)
      {
         if (introBattyEffect)
         {
            DoIntroTitleAffect();
         }
         else if (introMadeEffect)
         {
            DoIntroMadeByAffect();
         }
         else if (introNameEffect)
         {
            DoIntroNameAffect();
         }

      }

      /// <summary>
      /// Animates the title displayed in the intro
      /// </summary>
      private void DoIntroTitleAffect()
      {
         lblIntroBatty.Font = new Font(lblIntroBatty.Font.FontFamily, introBattyFontSize, FontStyle.Regular);
         lblIntroBatty.Left = (pnlIntro.Size.Width / ITEM_SIZE_SPLIT) - (lblIntroBatty.Size.Width / ITEM_SIZE_SPLIT);
         lblIntroBatty.Top = (pnlIntro.Size.Height / ITEM_SIZE_SPLIT) - (lblIntroBatty.Size.Height / ITEM_SIZE_SPLIT);

         introBattyFontSize += 1;
         if (introBattyFontSize > MAX_INTROFONT)
         {
            introBattyEffect = false;
            introMadeEffect = true;
            lblIntroBatty.Visible = false;
         }
      }

      /// <summary>
      /// Animates the made by text in the intro
      /// </summary>
      private void DoIntroMadeByAffect()
      {
         lblMadeBy.Top += MOVE_STEP;
         if (lblMadeBy.Top > pnlIntro.Size.Height + lblMadeBy.Size.Height)
         {
            introMadeEffect = false;
            lblMadeBy.Visible = false;
            introNameEffect = true;
         }
      }

      /// <summary>
      /// Animates the names displayed in the game intro
      /// </summary>
      private void DoIntroNameAffect()
      {
         lblDan.Left -= MOVE_STEP;
         lblAlex.Left -= MOVE_STEP;
         lblDerek.Left -= MOVE_STEP;
         lblLevon.Left += MOVE_STEP;
         lblMatthew.Left += MOVE_STEP;

         if (lblLevon.Left > pnlIntro.Size.Width)
         {
            introNameEffect = false;
            timerIntro.Enabled = false;
            SwitchToMenu();
         }
      }

      /// <summary>
      /// Switch to the Main Menu panel from any panel
      /// </summary>
      private void SwitchToMenu()
      {
         timerTitleFlash.Enabled = true;
         HidePanels();
         ToggleMenuItems();
         pnlMenu.Visible = true;
         this.Focus();
      }

      /// <summary>
      /// Hides restricted menu options when a game is running
      /// </summary>
      private void ToggleMenuItems()
      {
         if (gameRunning)
         {
            lblStart.Visible = false;
            lblEditor.Enabled = false;
            lblRestart.Visible = true;
            lblStart.Enabled = false;
            lblRestart.Enabled = true;
         }
         else
         {
            lblStart.Visible = true;
            lblEditor.Enabled = true;
            lblRestart.Visible = false;
            lblStart.Enabled = true;
            lblRestart.Enabled = false;
         }
      }

      /// <summary>
      /// Closes the game on button click
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void lblExit_Click(object sender, EventArgs e)
      {
         Application.Exit();
      }

      /// <summary>
      /// Navigates to the main game from menu on click
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void lblStart_Click(object sender, EventArgs e)
      {

         gameManager.Restart();
         gameManager.ChangeBarSize(trackPaddleSize.Value);
         levelEnd = false;
         pnlMenu.Visible = false;
         timerTitleFlash.Enabled = false;
         pnlLevel.Visible = true;
         gameRunning = true;
         timerGame.Enabled = true;
         SetToOptions();
         st.Start();
      }

      /// <summary>
      /// navigates to the instructions page from main menu on click
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void lblInstructions_Click(object sender, EventArgs e)
      {
         pnlMenu.Visible = false;
         pnlInstructions.Visible = true;
         timerTitleFlash.Enabled = false;
         if (panelStack.Count > 0)
            panelStack.Push(pnlInstructions);
      }

      /// <summary>
      /// navigates to the level editor from the main menu on click
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void lblEditor_Click(object sender, EventArgs e)
      {
         pnlMenu.Visible = false;
         timerTitleFlash.Enabled = false;
         pnlEditor.Visible = true;
         if (panelStack.Count > 0)
            panelStack.Push(pnlEditor);
         FillFromLevel();
      }

      /// <summary>
      /// navigates to the options menu from the main menu on click
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void lblOptions_Click(object sender, EventArgs e)
      {
         pnlMenu.Visible = false;
         timerTitleFlash.Enabled = false;
         pnlOptions.Visible = true;
         if (panelStack.Count > 0)
            panelStack.Push(pnlOptions);
      }

      /// <summary>
      /// Set labal color to its "Mouse Enter" state color
      /// </summary>
      /// <param name="currLabel"></param>
      private void SetButtonHoverColor(Label currLabel)
      {
         currLabel.ForeColor = Color.Yellow;
      }

      /// <summary>
      /// Set labal color to its normal state color
      /// </summary>
      /// <param name="currLabel"></param>
      private void SetButtonStandardColor(Label currLabel)
      {
         currLabel.ForeColor = Color.White;
      }

      /// <summary>
      /// Prevent user from selecting the first column in the controls table
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void dgvInstructions_SelectionChanged(object sender, EventArgs e)
      {
         if (dgvInstructions.CurrentCell.ColumnIndex == 0)
         {
            dgvInstructions.ClearSelection();
         }
      }

      /// <summary>
      /// Change control to a desired key when a key cell is selected
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void dgvInstructions_KeyDown(object sender, KeyEventArgs e)
      {
         if (dgvInstructions.SelectedCells.Count > 0)
         {
            int row = dgvInstructions.CurrentCell.RowIndex;
            string controlName = dgvInstructions.Rows[row].Cells[0].Value.ToString();
            Batty_2._0.Controls.Instance.ChangeKey(controlName, e.KeyCode, dgvInstructions.CurrentCell.ColumnIndex);
         }
      }

      /// <summary>
      /// Itereate between the colors displayed in the title animation
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void timerTitleFlash_Tick(object sender, EventArgs e)
      {
         lblTitle.ForeColor = colorArray[colorIndexMM];
         colorIndexMM = (colorIndexMM + 1) % colorArray.Length;
      }

      /// <summary>
      /// Returns to the main menu on click
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void BackButtonClick(object sender, EventArgs e)
      {
         SwitchToMenu();
         if (panelStack.Count > 0)
            panelStack.Pop();
      }

      /// <summary>
      /// Returns to the main menu from the main game 
      /// </summary>
      private void ExitGameCommand()
      {
         pnlLevel.Visible = false;
         if (!gameManager.Paused)
         {
            gameManager.TogglePause();
            ToggleTimerOn();
         }
         timerGame.Enabled = false;
         panelStack.Push(pnlMenu);
         SwitchToMenu();
         lblPaused.Visible = !lblPaused.Visible;
      }

      /// <summary>
      /// Return to the Panel at the top on the stack
      /// Switches beween paused main game and main menu
      /// </summary>
      private void GoBackLevelMenu()
      {
         Panel currPanel = panelStack.Pop();

         if (currPanel == pnlMenu)
         {
            currPanel.Visible = false;
            pnlLevel.Visible = true;
            timerGame.Enabled = true;
            timerTitleFlash.Enabled = false;
         }
         else
            SwitchToMenu();
      }

      /// <summary>
      /// Moves the highlighted menu item up one 
      /// </summary>
      private void MenuMoveUp()
      {
         if (menuItemIndex > 0)
         {
            SetButtonStandardColor(menuItems[menuItemIndex]);
            menuItemIndex -= 1;
            SetButtonHoverColor(menuItems[menuItemIndex]);
         }
      }

      /// <summary>
      /// Moves the highlighted menu item down one
      /// </summary>
      private void MenuMoveDown()
      {
         if (menuItemIndex < menuItems.Length - 1)
         {
            if (menuItemIndex > -1)
               SetButtonStandardColor(menuItems[menuItemIndex]);
            menuItemIndex += 1;
            SetButtonHoverColor(menuItems[menuItemIndex]);
         }
      }

      /// <summary>
      /// Helper method used to click on a menu item without mouse click
      /// </summary>
      private void MenuSelect()
      {
         if (menuItemIndex > -1)
         {
            Type thisType = this.GetType();
            MethodInfo method = thisType.GetMethod(menuItems[menuItemIndex].Name + "_Click", BindingFlags.Instance | BindingFlags.NonPublic);
            SetButtonStandardColor(menuItems[menuItemIndex]);
            method.Invoke(this, new Object[] { menuItems[menuItemIndex], new EventArgs() });
            menuItemIndex = -1;
            
         }
      }

      /// <summary>
      /// Changes the highlighted menu item by key press
      /// </summary>
      private void MenuKeyboardMove()
      {
         if (Batty_2._0.Controls.Instance.ControlWasPressed(Batty_2._0.Controls.ControlType.Up))
            MenuMoveUp();
         else if (Batty_2._0.Controls.Instance.ControlWasPressed(Batty_2._0.Controls.ControlType.Down))
            MenuMoveDown();
         else if (Batty_2._0.Controls.Instance.ControlWasPressed(Batty_2._0.Controls.ControlType.Shoot))
            MenuSelect();
      }

      /// <summary>
      /// Checks all non game control key presses 
      /// Calls the function for that key
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void GameForm_KeyPress(object sender, KeyPressEventArgs e)
      {
         if (keyBeingHandled)
            return;
         keyBeingHandled = true;

         if (Batty_2._0.Controls.Instance.ControlWasPressed(Batty_2._0.Controls.ControlType.Left) 
            || Batty_2._0.Controls.Instance.ControlWasPressed(Batty_2._0.Controls.ControlType.Right))
         {
            keyBeingHandled = false;
            return;
         }

         if (pnlLevel.Visible)
         {
            if (Batty_2._0.Controls.Instance.ControlWasPressed(Batty_2._0.Controls.ControlType.Shoot))
            {
               // Do nothing
            }
            else if (Batty_2._0.Controls.Instance.ControlWasPressed(Batty_2._0.Controls.ControlType.Exit))
            {
               ExitGameCommand();
            }
            else if (Batty_2._0.Controls.Instance.ControlWasPressed(Batty_2._0.Controls.ControlType.Pause))
            {
               lblPaused.Visible = !lblPaused.Visible;
               gameManager.TogglePause();
               ToggleTimerOn();
            }
            else if (Batty_2._0.Controls.Instance.ControlWasPressed(Batty_2._0.Controls.ControlType.Skip))
            {
               levelEnd = true;
               gameManager.gameState = GameManager.GameState.LevelEnd;
            }
            else if (Batty_2._0.Controls.Instance.ControlWasPressed(Batty_2._0.Controls.ControlType.Lives))
            {
               gameManager.ModifyLives(99);
            }

         }
         else if (panelStack.Count > 0)
         {
            if (Batty_2._0.Controls.Instance.ControlWasPressed(Batty_2._0.Controls.ControlType.Exit))
            {
               GoBackLevelMenu();
            }
         }
         else if (pnlMenu.Visible)
         {
            MenuKeyboardMove();
         }
         else if (pnlEndLevel.Visible)
         {
            if (Batty_2._0.Controls.Instance.ControlWasPressed(Batty_2._0.Controls.ControlType.Exit))
            {
               gameRunning = false;
               SwitchToMenu();
            }
            else
            {
               if (gameManager.gameState == GameManager.GameState.Lost)
               {
                  EndGame();
               }
               else if (gameManager.gameState == GameManager.GameState.LevelEnd)
                  NextLevel();
               else if (gameManager.gameState == GameManager.GameState.Won)
                  EndGame();
            }
         }
         else if (pnlEndHScore.Visible)
         {
            EndHighScoreKeyPress();
         }
         else if (pnlHighScore.Visible && gameManager.gameState == GameManager.GameState.Won)
         {
            pnlHighScore.Visible = false;
            RunPostGameScreen();
         }
         else if (pnlHighScore.Visible && gameManager.gameState == GameManager.GameState.Lost)
         {
            pnlHighScore.Visible = false;
            gameRunning = false;
            SwitchToMenu();
         }
         else if (pnlSrcCode.Visible)
         {
            timerSrcCode.Enabled = false;
            gameRunning = false;
            SwitchToMenu();
         }
         keyBeingHandled = false;
      }

      /// <summary>
      /// Handle transitioning after game is won
      /// </summary>
      private void EndGame()
      {
         pnlEndLevel.Visible = false;
         if (hScoreTable.IsHighScore(gameManager.Score))
         {
            Thread thread = new Thread(new ThreadStart(AudioManager.PlayHighScore));
            thread.Start();

            lblHScoreScore.Text = "Score: " + gameManager.Score.ToString();
            pnlEndHScore.Visible = true;
            txtName.Focus();
         }
         else
            ShowEndHighScore();
            
      }

      /// <summary>
      /// Handle displaying the source code after the game ends
      /// </summary>
      private void RunPostGameScreen()
      {
         //sourceCodeManager = new SourceCodeController();
         pnlSrcCode.Visible = true;
         SourceCodeController.GenerateTheCode();
         lblSrcCode.Text = SourceCodeController.GetCurrentCode();
         timerSrcCode.Enabled = true;
      }

      /// <summary>
      /// Switch to the next level in the game
      /// </summary>
      private void NextLevel()
      {
         levelEnd = false;
         gameManager.gameState = GameManager.GameState.InProgress;
         //gameManager.SkipLevel();
         pnlEndLevel.Visible = false;
         SetToOptions();
         pnlLevel.Visible = true;
         timerGame.Enabled = true;
         st.Start();

         Thread thread = new Thread(new ThreadStart(AudioManager.PlayOpeningSequence));
         thread.Start();
      }

      /// <summary>
      /// Navigates to the highscore panel from main menu on click
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void lblHighScore_Click(object sender, EventArgs e)
      {
         pnlMenu.Visible = false;
         timerTitleFlash.Enabled = false;
         pnlHighScore.Visible = true;
         timerHScoreFlash.Enabled = true;
         if (panelStack.Count > 0)
            panelStack.Push(pnlHighScore);
      }

      /// <summary>
      /// increments the color used in the high score animation
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void timerHScoreFlash_Tick(object sender, EventArgs e)
      {
         lblHScoreTitle.ForeColor = colorArray[colorIndexHS];
         colorIndexHS = (colorIndexHS + 1) % colorArray.Length;
      }

      /// <summary>
      /// Increments the in-game timer displayed in the main game
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void timerGame_Tick(object sender, EventArgs e)
      {
         if (FPSDebug)
         {
            lblFPS.Visible = true;
         }
         if (!levelEnd)
         {
            n++;
            if (st.ElapsedMilliseconds >= 500)
            {
               elapsed = st.ElapsedMilliseconds;
               FPS = (n * 1000) / st.ElapsedMilliseconds;
               st.Reset();
               st.Start();
               n = 0;
               lblFPS.Text = Math.Round(FPS).ToString();
               int secondsLeft = gameManager.DecrimenntTime(elapsed);
               int minutsLeft = secondsLeft / 60;
               secondsLeft = secondsLeft - (minutsLeft * 60);
               if (secondsLeft > 9)
               {
                  lblTime.Text = "Time: " + minutsLeft.ToString() + ":" + secondsLeft;
               }
               else
               {
                  lblTime.Text = "Time: " + minutsLeft.ToString() + ":" + "0" + secondsLeft;
               }
            }
            gameManager.Update();
            pnlLevel.Refresh();
            UpdateStats();
            if (gameManager.gameState != GameManager.GameState.InProgress)
               levelEnd = true;
         }
         else
         {
            LevelComplete();
            st.Stop();
         }
      }

      /// <summary>
      /// Displays the Level complete panel
      /// </summary>
      private void LevelComplete()
      {
         timerGame.Enabled = false;
         Thread.Sleep(500);
         pnlLevel.Visible = false;
         pnlEndLevel.Visible = true;
         AddTimeToScore();
         UpdatePostLevelPanel();
         pnlLevel.Refresh();
      }

      /// <summary>
      /// Sets values o nthe post-level panel to those for the completed level
      /// </summary>
      private void UpdatePostLevelPanel()
      {
         if (gameManager.gameState == GameManager.GameState.LevelEnd)
            lblEndStatus.Text = "Level Complete!";   
         else if (gameManager.gameState == GameManager.GameState.Lost)
            lblEndStatus.Text = "Game Over!";

         lblEndStatus.Left = pnlEndLevel.Width / 2 - lblEndStatus.Width / 2;
         lblEndScore.Text = "Score: " + gameManager.Score.ToString();
         GameManager.GameState currState = gameManager.gameState;
         gameManager.SkipLevel();

         if (gameManager.gameState == GameManager.GameState.Won)
         {
            lblEndStatus.Text = "You Win!!!";
         }
         else
         {
            gameManager.gameState = currState;
         }

         lblEndLevel.Text = "Level: " + (gameManager.Level - 1).ToString();

         //lblTime.Text = "Time: " + gameManager.LevelTime.ToString();
      }

      /// <summary>
      /// Adds 10 bonus points to the score as all the remaining 
      /// time from the level is counted
      /// </summary>
      private void AddTimeToScore()
      {
         if (gameManager.gameState == GameManager.GameState.LevelEnd)
         {
            int timeLeft = gameManager.DecrimenntTime(0);
            lblEndTime.Text = "Time: " + timeLeft.ToString();
            for (int i = timeLeft; i >= 0; i--)
            {
               gameManager.AddPoints(10);
               lblEndScore.Text = "Score: " + gameManager.Score.ToString();
               lblEndTime.Text = "Time: " + i.ToString();
               Thread.Sleep(10);
               pnlEndLevel.Refresh();
            }
         }
      }

      /// <summary>
      /// Updates the level number, number of lives displayed and score
      /// </summary>
      private void UpdateStats()
      {
         lblScore.Text = "Score: " + gameManager.Score.ToString();
         lblLevel.Text = "Level: " + gameManager.Level.ToString();

         switch (gameManager.Lives)
         {
            case 1:
               picLives1.Visible = true;
               picLives2.Visible = false;
               picLives3.Visible = false;
               break;
            case 2:
               picLives1.Visible = true;
               picLives2.Visible = true;
               picLives3.Visible = false;
               break;
            case 3:
               picLives1.Visible = true;
               picLives2.Visible = true;
               picLives3.Visible = true;
               break;
            default:
               picLives1.Visible = true;
               picLives2.Visible = true;
               picLives3.Visible = true;
               break;
         }
      }

      /// <summary>
      /// Paints all of the shapes coresponding to the game objects in gameManager
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void pnlLevel_Paint(object sender, PaintEventArgs e)
      {
         List<GameObject> objects = gameManager.GetObjects();
         foreach (GameObject obj in objects)
         {
            if (obj != null)
            {
               if (obj.type == GameObject.GameObjectType.BALL)
               {
                  brush.Color = obj.color;
                  e.Graphics.FillEllipse(brush, obj.X, obj.Y, obj.Width, obj.Height);
               }
               if (obj.type == GameObject.GameObjectType.BLOCK)
               {
                  brush.Color = obj.color;

                  e.Graphics.FillRectangle(brush, obj.X, obj.Y, obj.Width, obj.Height);
                  e.Graphics.DrawRectangle(pen, obj.X, obj.Y, obj.Width, obj.Height);
               }
               else if (obj.type == GameObject.GameObjectType.BAR)
               {
                  brush.Color = obj.color;
                  e.Graphics.FillRectangle(brush, obj.X, obj.Y, obj.Width, obj.Height);
                  e.Graphics.DrawImage(obj.image, obj.X, obj.Y, obj.Width, obj.Height);
               }
               else  
                  e.Graphics.DrawImage(obj.image, obj.X, obj.Y, obj.Width, obj.Height);
            }
         }
      }

      /// <summary>
      /// Sets the block color the user selected 
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void SetEditorBlockColor(object sender, EventArgs e)
      {
         editorManager.BlockColor = (sender as Button).BackColor;
         tblBlockPos.DefaultCellStyle.SelectionBackColor = editorManager.BlockColor;
      }

      /// <summary>
      /// Colors the blocks the user selecte with the mouse
      /// updates tos blocks in the level file 
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void tblBlockPos_MouseUp(object sender, MouseEventArgs e)
      {
         if (tblBlockPos.SelectedCells.Count > 0)
         {
            if (editorManager.BlockColor != Color.Black && editorManager.BlockHealth > 0)
            {
               foreach (DataGridViewCell cell in tblBlockPos.SelectedCells)
               {
                  cell.Style.BackColor = editorManager.BlockColor;
                  if (editorManager.BlockColor == Color.White)
                  {
                     editorManager.RemoveBlock(cell.RowIndex, cell.ColumnIndex);
                  }
                  else
                  {
                     editorManager.AddBlock(cell.RowIndex, cell.ColumnIndex);
                  }
                  
                  
               }
            }
            tblBlockPos.ClearSelection();
         }
      }

      /// <summary>
      /// changes the number of live used by the editor on click
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void lblEditorLives_Click(object sender, EventArgs e)
      {
         editorLivesCounter++;
         switch (editorLivesCounter % (MAX_LIVES))
         {
            case 0:
               picEdtHeart2.Visible = false;
               picEdtHeart3.Visible = false;
               break;
            case 1:
               picEdtHeart2.Visible = true;
               picEdtHeart3.Visible = false;
               break;
            case 2:
               picEdtHeart2.Visible = true;
               picEdtHeart3.Visible = true;
               break;
         }

         editorManager.BlockHealth = (editorLivesCounter % (MAX_LIVES)) + 1;
      }

      /// <summary>
      /// Helper method that clears all colors from the editor block and enemy tables
      /// </summary>
      private void ClearEditorTables()
      {
         foreach (DataGridViewRow row in tblBlockPos.Rows)
         {
            foreach (DataGridViewCell cell in row.Cells)
               cell.Style.BackColor = Color.White;
         }

         foreach (DataGridViewCell cell in tblEnemyPos.Rows[0].Cells)
         {
            cell.Style.BackColor = Color.White;
         }
      }

      /// <summary>
      /// clears the level being edited in editor panel
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void btnClear_Click(object sender, EventArgs e)
      {
         errProvider.Clear();
         ClearEditorTables();
         editorManager.ClearLevel();
      }

      /// <summary>
      /// toggles the sound in the game 
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void ToggleSound(object sender, EventArgs e)
      {
         if (lblSoundOpt.Text == "OFF")
         {
            lblSoundOpt.Text = "ON";
            lblSoundOpt.ForeColor = Color.Lime;
            AudioManager.Muted = false;
         }
         else
         {
            lblSoundOpt.Text = "OFF";
            lblSoundOpt.ForeColor = Color.Red;
            AudioManager.Muted = true;
         }

         gameManager.ToggleSound();
      }

      /// <summary>
      /// changes the max lives upon click in the options menu
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void lblOptionLives_Click(object sender, EventArgs e)
      {
         if (!gameRunning)
         {
            optLivesClickCount++;
            switch (optLivesClickCount % (MAX_LIVES))
            {
               case 0:
                  picOptionLives2.Visible = false;
                  picOptionLives3.Visible = false;
                  initLives = 1;
                  break;
               case 1:
                  picOptionLives2.Visible = true;
                  picOptionLives3.Visible = false;
                  initLives = 2;
                  break;
               case 2:
                  picOptionLives2.Visible = true;
                  picOptionLives3.Visible = true;
                  initLives = 3;
                  break;
            }
            gameManager.ModifyLives(initLives);
         }
      }

      /// <summary>
      /// increments the color used to draw the ball
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void ChangeBallColor(object sender, EventArgs e)
      {
         colorBallIndex = (colorBallIndex + 1) % colorArray.Length;
         picBallColor.BackColor = colorArray[colorBallIndex];
         gameManager.BallColor = colorArray[colorBallIndex];
      }

      /// <summary>
      /// increments the color drawn under the paddle
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void ChangePaddleColor(object sender, EventArgs e)
      {
         colorPaddleIndex = (colorPaddleIndex + 1) % paddleColors.Length;
         picPaddleColor.BackColor = paddleColors[colorPaddleIndex];
         gameManager.BarColor = paddleColors[colorPaddleIndex];
      }

      /// <summary>
      ///  Sets an initial paddle color
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void picPaddleColor_LoadCompleted(object sender, AsyncCompletedEventArgs e)
      {
         picPaddleColor.BackColor = paddleColors[colorPaddleIndex];
         gameManager.BarColor = paddleColors[colorPaddleIndex];
      }

      /// <summary>
      /// displays the curent paddle size 
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void trackPaddleSize_ValueChanged(object sender, EventArgs e)
      {
         gameManager.ChangeBarSize(trackPaddleSize.Value);
      }

      /// <summary>
      /// displays the current ball speed
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void trackBallSpeed_ValueChanged(object sender, EventArgs e)
      {
         gameManager.ChangeBallSpeed(trackBallSpeed.Value);
      }

      /// <summary>
      /// save the edited level to file 
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void btnSave_Click(object sender, EventArgs e)
      {
         errProvider.Clear();
         //editorManager.SetToNormal();
         if (editorManager.Save())
         {
            btnClear_Click(sender, e);
            gameManager.ReloadLevel();
         }
      }

      /// <summary>
      /// places enemies in the user selected cells in level editor table
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void tblEnemyPos_MouseUp(object sender, MouseEventArgs e)
      {
         if (tblEnemyPos.SelectedCells.Count > 0)
         {
            if(editorManager.BlockHealth > 0)
            {
               foreach (DataGridViewCell cell in tblEnemyPos.SelectedCells)
               {
                  SetEditorEnemyCell(cell);
               }
               tblEnemyPos.ClearSelection();
            }
         }
      }

      /// <summary>
      /// updates to color of an indevidual cell in the enemey table in level editor
      /// </summary>
      /// <param name="cell"></param>
      private void SetEditorEnemyCell(DataGridViewCell cell)
      {
         if (cell.Style.BackColor != Color.Purple)
         {
            editorManager.AddEnemy(cell.ColumnIndex);
            cell.Style.BackColor = Color.Purple;
         }
         else
         {
            editorManager.RemoveEnemy(cell.ColumnIndex);
            cell.Style.BackColor = Color.White;
         }
      }

      /// <summary>
      /// Changes the color of a hovered button
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void HandleLabelHover(object sender, EventArgs e)
      {
         SetButtonHoverColor(sender as Label);
      }

      private void HandleLabelExit(object sender, EventArgs e)
      {
         SetButtonStandardColor(sender as Label);
      }

      /// <summary>
      /// Helper method when starting new level
      /// Sets current option settings to current level
      /// </summary>
      private void SetToOptions()
      {
         gameManager.BallColor = picBallColor.BackColor;
         gameManager.BarColor = picPaddleColor.BackColor;
         gameManager.ChangeBallSpeed(trackBallSpeed.Value);
         gameManager.ChangeBarSize(trackPaddleSize.Value);

      }

      /// <summary>
      /// Loads the a level data from a user selected file into the editor table
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void btnLoad_Click(object sender, EventArgs e)
      {
         errProvider.Clear();
         string initPath = "../../../Batty 2.0/Resources/Levels";
         OpenFileDialog fileDialog = new OpenFileDialog();
         if (Directory.Exists(initPath))
            fileDialog.InitialDirectory = initPath;
         else
            fileDialog.InitialDirectory = @"C:\";

         if (fileDialog.ShowDialog() == DialogResult.OK)
         {
            string fileName = fileDialog.SafeFileName.Split('.')[0];
            try
            {
               int fileNum = Int32.Parse(fileName);
               btnClear_Click(sender, e);
               if (editorManager.Load(fileNum))
               {
                  FillFromLevel();
               }
               else
                  errProvider.SetError(btnLoad, "Invalid file name");
            }
            catch (Exception ex)
            {
               errProvider.SetError(btnLoad, "Invalid file name");
            }

         }
      }

      /// <summary>
      /// Helper method used to fill the editor tables with enemies and blocks
      /// </summary>
      private void FillFromLevel()
      {
         ClearEditorTables();
         GameObject[,] levelBlocks = editorManager.GetLevelBlocks();
         for (int i = 0; i < GameManager.NUM_COLS; i++)
         {
            for (int j = 0; j < GameManager.NUM_ROWS; j++)
            {
               if (levelBlocks[i, j] != null)
                  tblBlockPos.Rows[j].Cells[i].Style.BackColor = levelBlocks[i, j].color;
            }
         }
         int enemyCount = 0;
         foreach(GameObject obj in editorManager.GetLevelEnemies())
         {
            tblEnemyPos.Rows[0].Cells[enemyCount].Style.BackColor = Color.Purple;
            enemyCount++;
         }

      }

      /// <summary>
      /// Loads the default level into the editor table
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void btnDefault_Click(object sender, EventArgs e)
      {
         errProvider.Clear();
         btnClear_Click(sender, e);
         editorManager.SetToDefault();
         FillFromLevel();
      }

      /// <summary>
      /// Cleads the selection of the first cell upon creation of the data grid view
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void dgvHighScore_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
      {
         dgvHighScore.ClearSelection();
      }

      /// <summary>
      /// Handles the entry of high score names
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void txtName_KeyPress(object sender, KeyPressEventArgs e)
      {
         EndHighScoreKeyPress();
      }

      /// <summary>
      /// Handle key press while in the high score input view
      /// </summary>
      private void EndHighScoreKeyPress()
      {
         if (Batty_2._0.Controls.Instance.ControlWasPressed(Batty_2._0.Controls.ControlType.Exit))
         {
            gameRunning = false;
            SwitchToMenu();
         }
         else if (Batty_2._0.Controls.Instance.ControlWasPressed(Batty_2._0.Controls.ControlType.Shoot))
         {
            errProvider.Clear();
            if (txtName.TextLength == 3)
            {
               hScoreTable.AddHighScore(gameManager.Score, txtName.Text);
               dgvHighScore.DataSource = null;
               SetUpHighScoreTable(); //here
               dgvHighScore.Update();
               pnlEndHScore.Visible = false;
               this.Focus();
               ShowEndHighScore();
            }
            else
               errProvider.SetError(txtName, "Please enter a 3 character name");
         }

      }

      /// <summary>
      /// Shows the high scores at the end game screen
      /// </summary>
      private void ShowEndHighScore()
      {
         pnlHighScore.Visible = true;
         timerHScoreFlash.Enabled = true;
         lblBackHScore.Visible = false;
      }

      /// <summary>
      /// Clears the default selection on creation of the data grid view
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void tblBlockPos_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
      {
         tblBlockPos.ClearSelection();
         tblBlockPos.Update();
         if (tblBlockPos.SelectedCells.Count > 0)
            tblBlockPos.CurrentCell = null;
      }

      /// <summary>
      /// moves th source code up the screen
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void timerSrcCode_Tick(object sender, EventArgs e)
      {
         if (!srcCodeLbl2Active || srcCodeLblSwitched)
         {
            lblSrcCode.Top = lblSrcCode.Top - 2;
         }
         if (srcCodeLbl2Active || srcCodeLblSwitched)
         {
            lblSrcCode2.Top = lblSrcCode2.Top - 2;
         }

         if(!srcCodeLbl2Active && !srcCodeLblSwitched && lblSrcCode.Bottom <= this.Height)
         {
            srcCodeLblSwitched = true;
            srcCodeLbl2Active = true;
            lblSrcCode2.Text = SourceCodeController.GetNextCode();
         }
         else if(srcCodeLbl2Active && srcCodeLblSwitched && lblSrcCode.Bottom <= 0)
         {
            srcCodeLblSwitched = false;
            lblSrcCode.Top = this.Height;
         }
         else if (srcCodeLbl2Active && !srcCodeLblSwitched && lblSrcCode2.Bottom <= this.Height)
         {
            srcCodeLblSwitched = true;
            srcCodeLbl2Active = false;
            lblSrcCode.Text = SourceCodeController.GetNextCode();
         }
         else if (!srcCodeLbl2Active && srcCodeLblSwitched && lblSrcCode2.Bottom <= 0)
         {
            srcCodeLblSwitched = false;
            lblSrcCode2.Top = this.Height;
         }
      }

      /// <summary>
      /// Starts a new instance of the main game on click
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void lblRestart_Click(object sender, EventArgs e)
      {
         lblPaused.Visible = false;
         pnlMenu.Visible = false;
         pnlLevel.Visible = false;
         gameRunning = false;
         gameManager.Restart();
         SwitchToMenu();
         lblStart_Click(sender, e);
         SetToOptions();
      }

      /// <summary>
      /// Starts and stops the stopwatch 
      /// </summary>
      private void ToggleTimerOn()
      {
         if(st.IsRunning)
         {
            st.Stop();
            st.Reset();
         }
         else
         {
            st.Start();
         }
      }
   }
}
