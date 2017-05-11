using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace firstForms
{
    struct Cell
    {
        public bool is_abyss;
        public Int32 x, y;
        public Color Clr;

        public Cell(bool flag, Int32 x_, Int32 y_, Color Clr_)
        {
            is_abyss = flag;
            x = x_;
            y = y_;            
            Clr = Clr_;
        }
    };
    struct Ball
    {
        public Int32 x, y, rad_x, rad_y;
        public Color circuit, inside;
        public Ball(Int32 x_ , Int32 y_ , Int32 rad_x_ , Int32 rad_y_ , Color circ_ , Color ins_ )
        {
            x = x_;
            y = y_;
            rad_x = rad_x_;
            rad_y = rad_y_;
            circuit = circ_;
            inside = ins_;
        }
    };
    struct Wall
    {
        public Int32 x, y;
        public bool Free_way;
        public Wall(Int32 x_ = 0, Int32 y_ = 0, bool Free_way_ = true)
        {
            x = x_;
            y = y_;
            Free_way = Free_way_;
        }
    };
    struct RPtable
    {
        public Int32 Points;
        public TimeSpan Tm;
        public RPtable(Int32 P, TimeSpan tmp)
        {
            Tm = tmp;
            Points = P;
        }

    };
    //enum m
    class RunnerGame
    {        
        RPtable[] At = new RPtable[20];
                        
        Graphics graphicContent;
        Form1 graphicPanel;
        Bitmap myBitmap;
        //Form1 frm;

        ToolStripStatusLabel toolScore, toolTime;

        Cell[,] Field;        

        Int32 Score, Dif;
        public Int32 Size_cell_width, Size_cell_height,
		    Cnt_block_height, Cnt_block_width,
		    Shift, Speed, Cur_line,
		    Jump_value, Jump_power,
            //Hours, Minutes, Seconds,
		    Cnt_walls;
        bool[] losted = new bool[3];
        bool[] Points_add = new bool[4];
        Color Clr_background, Clr_road;//colors

        Ball Ball_Truly, Ball_Falsed, Ball_Shadow;
        Wall[] The_wall = new Wall[3];
        public bool Is_fly, Jump_dest, Is_wall, records, Is_cleared;//balls

        //String Nm, Caption;
        Double I;//false ball
        DateTime Begin_time;//game begin
        //DateTime Cur_time;//time span
        TimeSpan tempT, additionTime;
	    //recors
        Int32 COUNT_RECORDS, J, tempP;
        
	    //Disco
        Int32 Disco_power, Disco_value;
        bool Is_disco;
        //
        Random rnd;
        //
        public Timer timer;
        public RunnerGame(Form1 incomingPanel, ToolStripStatusLabel toolScore, ToolStripStatusLabel toolTime)
        {            
            graphicPanel = incomingPanel;
            //
            this.toolScore = toolScore;
            this.toolTime = toolTime;
            //
            //graphicContent = incomingPanel.CreateGraphics();
            //graphicContent.SmoothingMode = SmoothingMode.AntiAlias;//AntiAlias
            //graphicContent.PixelOffsetMode = PixelOffsetMode.Half;
            //
            rnd = new Random();
            //
            Dif = 3;
            Jump_power = 50;//35
            Disco_power = 30;
            Size_cell_width = 60;
            Size_cell_height = 40;
            Cnt_block_height = 3;
            Cnt_block_width = (600 / Size_cell_width) + 1;
            Shift = 12;
            Clr_background = Color.Black;
            Clr_road = Color.White;
            //
            timer = new Timer() { Interval = 1000 / 30 };
            timer.Tick += new EventHandler(Timer_Tick);
            //
            New_game();
            //
            DrawField();
        }
        public void StartGame() {
            Begin_time = DateTime.Now;
            timer.Start();
        }
        public void PauseGame()
        {
            if (timer.Enabled)
            {
                timer.Stop();
                additionTime += DateTime.Now - Begin_time;
            }
        }
        public void ResetGame() {
            timer.Stop();
            New_game();            
            DrawField();
        }
        void Timer_Tick(object sender, EventArgs e) { 
            I+= Math.PI * 2 / ((8 - Speed) * 20);//50 100 150
	        for (int i = 1; i <= 4; i++)
		        if (I >= Math.PI * (0.5 * i) && !Points_add[i - 1]){
			        Score++;
			        Points_add[i - 1] = true;
		        }
	        if (I >= Math.PI * 2){
		        I = 0;
		        for (int i = 0; i < 4; i++)
			        Points_add[i] = false;
	        }
	        if (!Do_shift()){                
                timer.Stop();
                At[0] = new RPtable(Score, tempT);//////////////////////////////
                SaveBinaryFormat("Records.dat");             
                String str;
                str = "Вы набрали " + Score.ToString() + " очков за " + string.Format("{0:00}:{1:00}:{2:00}",
                    tempT.Minutes + additionTime.Minutes, tempT.Seconds + additionTime.Seconds, tempT.Milliseconds + additionTime.Milliseconds);
                MessageBox.Show(str, "Конец игры", MessageBoxButtons.OK);
                New_game();
	        }	      
            DrawField();

        }
        void SaveBinaryFormat(string fileName)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(fileName, FileMode.OpenOrCreate)))
            {
                writer.Write(COUNT_RECORDS);
                for (int j = 0; j < COUNT_RECORDS; j++){
                    writer.Write(At[j].Points);
                    writer.Write(At[j].Tm.ToString());
                }
            }
        }
        void LoadFromBinaryFile(string fileName)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(fileName, FileMode.Open)))
            {
                J = reader.ReadInt32();
                COUNT_RECORDS += (J == 20) ? --J : J;
                for (int j = 0; j < J; j++) {
                    tempP = reader.ReadInt32();
                    tempT = TimeSpan.Parse(reader.ReadString());
                    At[j + 1] = new RPtable(tempP, tempT);
                }
            }
        }


        void New_game(){
            Begin_time = DateTime.Now;
            additionTime = TimeSpan.Zero;
            records = false;
	        I = 0;
	        Score = Jump_value = Cnt_walls = Disco_value = 0;
		
	        COUNT_RECORDS = Cur_line = 1;
		
	        Speed = Dif;//3 5 7
	        Field = new Cell[Cnt_block_height, Cnt_block_width];	        

	        losted[0] = losted[1] = losted[2]  
	        = Points_add[0] = Points_add[1] = Points_add[2] = Points_add[3]
	        = Is_fly = Is_wall = Jump_dest 
	        = Is_disco = Is_cleared =/*The_wall[0].Free_way = The_wall[1].Free_way = The_wall[2].Free_way =*/ false;
	        The_wall[0].Free_way = The_wall[1].Free_way = The_wall[2].Free_way = true;

	        for (int i=0; i < Cnt_block_height; i++)
                for (int j = 0; j < Cnt_block_width; j++) Field[i, j] = new Cell(false, 600 - (j * Size_cell_width) - (i * Shift), 120 + (i * Size_cell_height), Clr_road);

            Create_ball(out Ball_Truly, 100, 140, 50, 50, Color.Gray, Color.Black);
            Create_ball(out Ball_Falsed, Ball_Truly.x + (Ball_Truly.rad_x / 2) - 5, Ball_Truly.y + (Ball_Truly.rad_y / 2) - 5, 10, 10, Color.Black, Color.Black);
            Create_ball(out Ball_Shadow, 80, 160, 65, 35, Color.Black, Color.Black);

	        At[0] = new RPtable();
            LoadFromBinaryFile("Records.dat");            
        }
        void Create_ball(out Ball ball, Int32 x, Int32 y, Int32 rad_x, Int32 rad_y, Color inside, Color circuit)
        {
	        ball.x = x;
	        ball.y = y;
	        ball.rad_x = rad_x;
	        ball.rad_y = rad_y;
	        ball.inside = inside;
	        ball.circuit = circuit;
        }
        public void Move(Int32 move_x, Int32 move_y)
        {
	        if (move_y < 0 && Cur_line > 0)
	        {
                Move_Ball(ref Ball_Truly, Shift, -Size_cell_height);
                Move_Ball(ref Ball_Falsed, Shift, -Size_cell_height);
                Move_Ball(ref Ball_Shadow, Shift, -Size_cell_height);
		        Cur_line--;
	        }
	        else if (move_y > 0 && Cur_line < Cnt_block_height - 1){
                Move_Ball(ref Ball_Truly, -Shift, Size_cell_height);
                Move_Ball(ref Ball_Falsed, -Shift, Size_cell_height);
                Move_Ball(ref Ball_Shadow, -Shift, Size_cell_height);
		        Cur_line++;
	        }
        }
        void Move_Ball(ref Ball ball, Int32 move_x, Int32 move_y)
        {
	        ball.x += move_x;
	        ball.y += move_y;
        }
        void Do_fly(){
	        if (Is_fly){
		        if (!Jump_dest){
			        Jump_value += Speed;
                    Move_Ball(ref Ball_Truly, 0, -(Speed));
                    Move_Ball(ref Ball_Falsed, 0, -(Speed));
                    Move_Ball(ref Ball_Shadow, -1, 0);
			        if (Jump_value >= Jump_power) Jump_dest = true;
		        }
		        else{
			        Jump_value -= Speed;
                    Move_Ball(ref Ball_Truly, 0, Speed);
                    Move_Ball(ref Ball_Falsed, 0, Speed);
                    Move_Ball(ref Ball_Shadow, 1, 0);
		        }
	        }
	        if (Jump_dest && Jump_value == 0){
		        Is_fly = Jump_dest = false;
	        }
        }
        bool Do_shift(){
	        for (int i = 0; i < Cnt_block_height; i++)
                for (int j = 0; j < Cnt_block_width; j++)
                {
			        Field[i, j].x -= Speed;
			        if (Field[i, j].x + Size_cell_width <= 0){//За гранью
				        Field[i, j].x = 600;// FIELD_WIDTH
				        Field[i, j].is_abyss = false;
				        Field[i, j].Clr = Clr_road;
				        //losted[i] = true;
			        }
		        }	
	        Do_fly();
	        Generate_cell();
	        Generate_Wall();
	        Need_Disco();
	        if ((!Is_fly && !Check_Collision()) || In_diap_wall(Ball_Truly))//((CMainFrame*)AfxGetMainWnd())->SetWindowTextW(L"Провал!!!");
		        return false;
	        else//((CMainFrame*)AfxGetMainWnd())->SetWindowTextW(L"Ball");		
		        return true;
        }
        bool Check_abyss(Int32 point, Int32 i){
	        for (int j = 0; j < Cnt_block_width; j++)
		        if (Field[i, j].x == point)
			        if (Field[i, j].is_abyss == false)
				        return true;
			        else
				        return false;
	        return false;
        }
        bool Check_Collision(){
	        for (int i = 0; i < Cnt_block_width; i++)
		        if (In_diap(Ball_Truly.x + (Ball_Truly.rad_x / 2), Field[Cur_line, i].x))
			        if (In_diap_cell(Ball_Truly.x + (Ball_Truly.rad_x / 2), Ball_Truly.y + Ball_Truly.rad_y, Field[Cur_line, i]))
				        if (Field[Cur_line, i].is_abyss == true)
					        return false;
	        return true;
        }
        bool In_diap(Int32 x, Int32 x_cell)
        {
	        return (x >= x_cell - Shift && x <= x_cell + Size_cell_width + Shift) ? true : false;
        }
        bool In_diap_cell(Int32 x, Int32 y, Cell Cell)
        {
            Int32 d1 = ((x - Cell.x) * ((Cell.y + Size_cell_height) - Cell.y)) - ((y - Cell.y) * ((Cell.x - Shift) - Cell.x));
	        if (d1 == 0)
		        return true;
	        else if (d1 < 0)
		        return false;
	        else{
		        Int32 d2 = ((x - (Cell.x + Size_cell_width))*((Cell.y + Size_cell_height) - Cell.y)) - ((y - Cell.y)*((Cell.x + Size_cell_width - Shift) - (Cell.x + Size_cell_width)));		
		        return (d2 == 0 || d2 < 0) ? true : false;					
	        }
        }
        bool In_diap_wall(Ball bl){
	        if (Is_wall){
		        if (The_wall[Cur_line].Free_way == false && 
			        ((bl.x + bl.rad_x - (Shift / 2) >= The_wall[Cur_line].x && bl.x + bl.rad_x - (Shift / 2) <= The_wall[Cur_line].x + Shift) ||
                    (bl.x + (Shift / 2) <= The_wall[Cur_line].x + Shift && bl.x + (Shift / 2) >= The_wall[Cur_line].x)))
			        return true;
		        else
			        return false;
	        }
	        else 
		        return false;	
        }
        bool Need_create(){
            return (rnd.Next(0, 10) < (Dif * 2)) ? true : false;
        }
        void Need_Disco(){
	        if (Score % ((10 / Dif) * 5) == 0 && !Is_disco && Score != 0)
		        if (rnd.Next(0, 10) < 10)
			        Is_disco = true;	
        }

        void Generate_cell(){
	        for (int i = 0; i < Cnt_block_height; i++)
		        for (int j = 0; j < Cnt_block_width; j++)
			        if (Field[i, j].x == 600 && Field[i, j].is_abyss == false){//FIELD_WIDTH
				        if (Need_create()){
                            for (int z = 0; z < Cnt_block_height; z++)
						        if (z != i){
							        if (Check_abyss(Field[z, j].x, z)){
								        if (Check_abyss(Field[i, j].x - Size_cell_width, i) || Check_abyss(Field[i, j].x - (2 * Size_cell_width), i)){
									        Field[i, j].is_abyss = true;
									        Field[i, j].Clr = GenerateColrBlock();
								        }								
							        }
						        }										
					        }				
				        }		
        }
        bool Check_right_wall(Int32 x, Int32 y){
            for (int i = 0; i < Cnt_block_width; i++)
		        if (Field[y, i].x == x) return true;	
	        return false;
        }
        void Generate_Wall(){
	        if (Score % 10 == 0 && !Is_wall){
		        Is_wall = true;
		        while (Cnt_walls < Cnt_block_height - 1){
			        Int32 temp = rnd.Next(0, Cnt_block_height);
			        if (The_wall[temp].Free_way == true){
				        The_wall[temp].x = 600 - (temp * Shift);//FIELD_WIDTH
				        The_wall[temp].y = 120 + ((temp+1) * 40) - 80;
				        The_wall[temp].Free_way = false;
				        Cnt_walls++;
			        }
		        }		
	        }
	        else if (Is_wall){
		        for (int i=0; i < Cnt_block_height; i++)
			        if (The_wall[i].Free_way == false){
				        The_wall[i].x -= Speed;
				        if (The_wall[i].x + Shift <= 0){
					        The_wall[i].Free_way = true;
					        Cnt_walls--;
				        }
				        if (Cnt_walls == 0)
					        Is_wall = false;
			        }		
	        }
        }
        Color GenerateColrBlock()
        {
	        while (true)
                switch (rnd.Next(0, 6))
                {
                    case 0:
                        return Color.Blue;
                    case 1:
                        return Color.Green;
                    case 2:
                        return Color.Cyan;
                    case 3:
                        return Color.Red;
                    case 4:
                        return Color.Gray;
                    case 5:
                        return Color.Yellow;
	            }
        }
        public void DrawField(){
            graphicPanel.AutoScrollMinSize = new Size(0, 0);

            myBitmap = new Bitmap(graphicPanel.Width, graphicPanel.Height);
            graphicContent = Graphics.FromImage(myBitmap);
            graphicContent.SmoothingMode = SmoothingMode.HighQuality;//AntiAlias
            graphicContent.InterpolationMode = InterpolationMode.High;

            graphicContent.Clear(Clr_background);

            Is_cleared = false;
            records = false;
            for (int i = 0; i < Cnt_block_height; i++)
                for (int j = 0; j < Cnt_block_width; j++) Show_Figure(Field[i, j]);
            if (Is_wall)
            {
                for (int i = 0; i < Cnt_block_height; i++){
			        if (The_wall[i].Free_way == false){
				        Point[] pts = new Point[4]{
					        new  Point(The_wall[i].x, The_wall[i].y ),
					        new  Point(The_wall[i].x + Shift, The_wall[i].y + Size_cell_height - 80 ),
					        new  Point(The_wall[i].x + Shift, The_wall[i].y + Size_cell_height ),
					        new  Point(The_wall[i].x, The_wall[i].y + 80 )};
                        graphicContent.FillPolygon(new SolidBrush(Clr_road), pts);
                        graphicContent.DrawPolygon(new Pen(Clr_background), pts);
			        }
			        else{
                        

				        Show_ball(Ball_Shadow, Ball_Shadow.x, Ball_Shadow.y, Ball_Shadow.rad_x, Ball_Shadow.rad_y);
				        Show_ball(Ball_Truly, Ball_Truly.x, Ball_Truly.y, Ball_Truly.rad_x, Ball_Truly.rad_y);

                        GraphicsState graphicState = graphicContent.Save();
                        graphicContent.TranslateTransform(Ball_Truly.x + Ball_Truly.rad_x / 2, Ball_Truly.y + Ball_Truly.rad_y / 2);

                        graphicContent.RotateTransform(Convert.ToSingle(I * 180 / Math.PI));
                        graphicContent.TranslateTransform(9, 9);
                        Show_ball(Ball_Falsed, 0, 0, Ball_Falsed.rad_x, Ball_Falsed.rad_y);

                        graphicContent.Restore(graphicState);
                    }
		        }
            }
            else {
                Show_ball(Ball_Shadow, Ball_Shadow.x, Ball_Shadow.y, Ball_Shadow.rad_x, Ball_Shadow.rad_y);
                Show_ball(Ball_Truly, Ball_Truly.x, Ball_Truly.y, Ball_Truly.rad_x, Ball_Truly.rad_y);

                GraphicsState graphicState = graphicContent.Save();
                graphicContent.TranslateTransform(Ball_Truly.x + Ball_Truly.rad_x / 2, Ball_Truly.y + Ball_Truly.rad_y / 2);

                graphicContent.RotateTransform(Convert.ToSingle(I * 180 / Math.PI));
                graphicContent.TranslateTransform(9, 9);
                Show_ball(Ball_Falsed, 0, 0, Ball_Falsed.rad_x, Ball_Falsed.rad_y);

                graphicContent.Restore(graphicState);
            }
            graphicPanel.BackgroundImage = myBitmap;
            graphicContent.Dispose();
            toolScore.Text = "Score: " + Score.ToString();
            tempT = DateTime.Now - Begin_time + additionTime;            
            toolTime.Text = string.Format("Time: {0:00}:{1:00}:{2:00}", tempT.Minutes, tempT.Seconds, tempT.Milliseconds);
        }
        void Show_ball(Ball bl, Int32 x, Int32 y, Int32 x2, Int32 y2)
        {        
            SolidBrush brush = new SolidBrush(bl.inside);
            Pen pen = new Pen(bl.circuit, 1);

            graphicContent.FillEllipse(brush, x, y, x2, y2);
            //graphicContent.DrawEllipse(pen, x, y, x2, y2);

            brush.Dispose();
            pen.Dispose();
        }
        void Show_Figure(Cell obj){
	        if (Is_disco)
		        if (Disco_value++ == Disco_power){
			        Is_disco = false;
			        Disco_value = 0;
		        }	
	        SolidBrush brush = (Is_disco) ? new SolidBrush(GenerateColrBlock()) : new SolidBrush(obj.Clr);	  
            Pen pen = new Pen(Clr_background, 1);
	        Point[] pts = new Point[4]{
		        new Point( obj.x, obj.y ),
		        new Point( obj.x + Size_cell_width, obj.y ),
		        new Point( obj.x + Size_cell_width - Shift, obj.y + Size_cell_height ),
		        new Point( obj.x - Shift, obj.y + Size_cell_height )};
            graphicContent.FillPolygon(brush, pts);                    
            //graphicContent.DrawPolygon(pen, pts);

            brush.Dispose();
            pen.Dispose();
        }
        public void Show_Records() {//Scroll
            graphicContent = graphicPanel.CreateGraphics();
            if (!Is_cleared)
            {
                graphicPanel.BackgroundImage.Dispose();
                myBitmap = new Bitmap(graphicPanel.Width, graphicPanel.Height);
                graphicPanel.BackgroundImage = myBitmap;                
                Is_cleared = true;
            }                                   

            Rectangle rectSize, rectCells;
            Int32 widthFirst, height, width;

            height = 25;

            rectSize = new Rectangle();

            rectSize.Width = graphicPanel.Size.Width;
            rectSize.Height = graphicPanel.Size.Height;

            width = (rectSize.Width - 17);// - 1

            widthFirst = width / 2;

            Int32 I = (At[0].Points != 0 && At[0].Tm != TimeSpan.Zero) ? 0 : 1;
            graphicPanel.AutoScrollMinSize = new Size(rectSize.Width - 20, height * (COUNT_RECORDS + 2));

            rectCells = new Rectangle(0, 25, rectSize.Width - 17, height);
            DrawCell("Таблица рекордов", rectCells, 1, Color.Green, Color.Black);

            rectCells = new Rectangle(0, 25 + height, widthFirst, height);
            DrawCell("Балы", rectCells, 1, Color.Green, Color.Black);

            rectCells = new Rectangle( widthFirst, 25 + height, widthFirst, height);
            DrawCell("Время", rectCells, 1, Color.Green, Color.Black);

            for (; I < COUNT_RECORDS; I++)
            {
                rectCells.Offset(-widthFirst, height);
                DrawCell(string.Format("{0}", At[I].Points), rectCells/*, 13*/, 1, Color.Black, Color.Black);
                rectCells.Offset(widthFirst, 0);
                DrawCell(string.Format("{0:00}:{1:00}:{2:00}", At[I].Tm.Minutes, At[I].Tm.Seconds, At[I].Tm.Milliseconds), rectCells, 1, Color.Black, Color.Black);
            }
        }
        void DrawCell(String text, Rectangle lpRect, Int32 widthp, Color clrText, Color clrBorder)
        {            
            
            SolidBrush drawBrush = new SolidBrush(clrText);
            Font drawFont = new Font("Arial", 14);
            StringFormat drawFormat = new StringFormat();
            //drawFormat.FormatFlags = StringFormatFlags.DirectionVertical;
            drawFormat.Alignment = StringAlignment.Center;

            Pen penBorder = new Pen(clrText, widthp);
            Point pt = graphicPanel.AutoScrollPosition;

            graphicContent.DrawString(text, drawFont, drawBrush, lpRect.X + (lpRect.Width / 2) + pt.X, lpRect.Y + pt.Y, drawFormat);
        }
    }
}
