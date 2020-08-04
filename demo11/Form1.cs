using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using SharpGL;

namespace demo11
{
    public partial class Form1 : Form
    {
        public float angle;                  // 机器人绕视点旋转的角度
        float[] legAngle = new float[2];     // 腿的当前旋转角度
        float[] armAngle = new float[2];     // 胳膊的当前旋转角度

        bool leg1 = true;                    // 机器人腿的状态，true向前，flase向后
        bool leg2 = false;
        bool arm1 = true;
        bool arm2 = false;

        private float rotation = 0.0f;

        //记录相关的参数
        double radius, rx, ry, cx, cy, upz;

        double eyex, eyey, eyez;

        double centerx, centery, centerz;

        double nx, ny, nz;

        int mx, my;

        bool btn_on_move, btn_change_view;

        const double CV_PI = 3.1415926;

        double scaleN=1.0;

        uint glList;

        public Form1()
        {
            InitializeComponent();
            openGLControl1.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.openGlMouseWheel);

            angle = 0.0f;                    // 设置初始角度为0
            legAngle[0] = legAngle[1] = 0.0f;
            armAngle[0] = armAngle[1] = 0.0f;

            Console.WriteLine("fsdfsa");
            OpenGL gl = openGLControl1.OpenGL;
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

            gl.LoadIdentity();
            //gl.Rotate(rotation, 0.0f, 1.0f, 0.0f);
            drawGrid(ref gl);
            DrawPCDFile(ref gl);
        }

        public void openGlMouseWheel(object sender, MouseEventArgs e)
        {
            //if (glList <= 0) return;
            if (e.Delta > 0)
                scaleN += 0.1F;
            else
                scaleN -= 0.1F;
            //if (scaleN > 10) scaleN = 10;
            if (scaleN <= 0) scaleN = 0.1;
            Console.WriteLine("openGlMouseWheel==>scale:" + scaleN);
            UpdateDrawData();
        }

        public void UpdateDrawData()
        {
            OpenGL gl = openGLControl1.OpenGL;
            gl.Clear(OpenGL.GL_DEPTH_BUFFER_BIT | OpenGL.GL_COLOR_BUFFER_BIT);
            // 深度截取模式 3d显示在外面时也能显示
            gl.Enable(OpenGL.GL_DEPTH);
            // 以下操作基于原点
            gl.LoadIdentity();
            gl.PushMatrix();
            // rx ry 是定义球体与h轴 与y轴的角度
            eyex = radius * Math.Cos(ry) * Math.Cos(rx);
            eyey = radius * Math.Cos(ry) * Math.Sin(rx);
            eyez = radius * Math.Sin(ry);
            centerx = radius * Math.Cos(cy) * Math.Cos(cx);
            centery = radius * Math.Cos(cy) * Math.Sin(cx);
            centerz = radius * Math.Sin(cy);
            Console.WriteLine("eye xyz: " + eyex + "," + eyey + "," + eyez + ",");
            gl.LookAt(eyex, eyey, eyez, 0, 0, 0, 0, 0, upz);
            gl.Translate(nx, ny, nz);
            // 右键移动视野待完善
            //gl.Translate(centerx, centery, centerz);
            gl.Scale(scaleN, scaleN, scaleN);
            drawGrid(ref gl);
            gl.CallList(glList);
            gl.Finish();
            gl.PopMatrix();
        }

        void drawGrid(ref OpenGL gl)
        {
            //绘制过程
            gl.PushAttrib(OpenGL.GL_CURRENT_BIT);  //保存当前属性
            gl.PushMatrix();                        //压入堆栈
            gl.Translate(0f, -20f, 0f);
            gl.Color(0f, 0f, 1f);
            //glList = gl.GenLists(1);
            //gl.NewList(glList, OpenGL.GL_COMPILE);
            gl.Begin(OpenGL.GL_LINES);
            //在X,Z平面上绘制网格
            for (float i = -50; i <= 50; i += 1)
            {
                //绘制线
                {
                    //if (i == 0)
                    //    gl.Color(0f, 1f, 0f);
                    //else
                    //    gl.Color(0f, 0f, 1f);

                    //X轴方向 连线(-50, 0,30)与(50, 0, 30)
                    //xy平面
                    gl.Vertex(-50f, 0f, i);
                    gl.Vertex(50f, 0f, i);
                    //Z轴方向
                    //gl.Vertex(i, 0f, -40f);
                    //gl.Vertex(i, 0f, 40f);

                }
                
            }

            gl.LineWidth(2);
            //X轴 red
            gl.Color(1.0f, 0.0f, 0.0f);
            gl.Vertex(500, 0, 0);
            gl.Vertex(-500, 0, 0);
            //Y轴 green
            gl.Color(0.0f, 1.0f, 0.0f);
            gl.Vertex(0, 500, 0);
            gl.Vertex(0, -500, 0);
            //Z轴 blue
            gl.Color(0.0f, 0.0f, 1.0f);
            gl.Vertex(0, 0, 500);
            gl.Vertex(0, 0, -500);
            gl.End();
            //gl.EndList();
            gl.PopMatrix();
            gl.PopAttrib();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            eyex = (double)this.numericUpDown1.Value;
            UpdateDrawData();
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            centerx = (double)this.numericUpDown4.Value;
            UpdateDrawData();
        }

        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            centery = (double)this.numericUpDown5.Value;
            UpdateDrawData();
        }

        private void numericUpDown6_ValueChanged(object sender, EventArgs e)
        {
            centerz = (double)this.numericUpDown6.Value;
            UpdateDrawData();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void openGLControl1_Load(object sender, EventArgs e)
        {

        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            eyey = (double)this.numericUpDown2.Value;
            UpdateDrawData();
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            eyez = (double)this.numericUpDown3.Value;
            UpdateDrawData();
        }

        private void openGLControl1_OpenGLDraw(object sender, PaintEventArgs e)
        {
            //// 实际运行是每一帧都会进这个函数
            //OpenGL gl = openGLControl1.OpenGL;
            //gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

            //gl.LoadIdentity();
            //gl.Rotate(rotation, 0.0f, 1.0f, 0.0f);

            //drawGrid(gl);
            ////DrawRobot(ref gl, 0, 0, 0);
            //rtest(ref gl, 0, 0, 0);
            //rotation += 1.0f;
        }

        private void openGLControl1_OpenGLInitialized(object sender, EventArgs e)
        {
            //OpenGL gl = openGLControl1.OpenGL;
            //gl.ClearColor(0, 0, 0, 0);
        }

        private void openGLControl1_Resized(object sender, EventArgs e)
        {
            OpenGL gl = openGLControl1.OpenGL;

            gl.MatrixMode(OpenGL.GL_PROJECTION);

            gl.LoadIdentity();
            gl.Perspective(60.0f, (double)Width / (double)Height, 0.01, 100.0);
            //gl.LookAt(-5, 5, 15, 0, 0, 0, 0, 1, 0);
            gl.MatrixMode(OpenGL.GL_MODELVIEW);
        }

        internal void DrawPCDFile(ref OpenGL Gl)
        {
            string pcd_file_path = "G:/blazarlin/pcl_project/pcl_test/pcl_test/rabbit.pcd";
            //string choose_file = "";
            //OpenFileDialog openFileDialog1 = new OpenFileDialog();
            //openFileDialog1.Filter = "ext files (*.pcd)|*.pcd|All files(*.*)|*>**";
            //openFileDialog1.FilterIndex = 1;
            //openFileDialog1.RestoreDirectory = true;
            //Stream myStream = null;
            //if (openFileDialog1.ShowDialog() == DialogResult.OK)
            //{
            //    if ((myStream = openFileDialog1.OpenFile()) != null)
            //    {
            //        choose_file = openFileDialog1.FileName;
            //    }
            //    else
            //    {
            //        return;
            //    }
            //}
            //else
            //{
            //    return;
            //}
            

            openGLControl1.Visible = true;
            string[] lines = System.IO.File.ReadAllLines(pcd_file_path);
            System.Console.WriteLine("txt data = ");
            double[,] pts = new double[lines.Length, 3];

            Gl.PushMatrix();
            Gl.Translate(0, 0, 0);

            glList = Gl.GenLists(1);
            //加入到list中
            Gl.NewList(glList, OpenGL.GL_COMPILE);
            //绘制点的形式
            Gl.Begin(OpenGL.GL_POINTS);
            double max_x = -99999, max_y = max_x, max_z = max_x;
            double min_x = 99999, min_y = min_x, min_z = min_x;

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];

                if (i > 10)
                {
                    // Console.WriteLine(i + "\t" + line);
                    string[] coord_data = line.Split(' ');

                    if (coord_data.Length >= 3)
                    {
                        double x = Convert.ToDouble(coord_data[0])*50;
                        double y = Convert.ToDouble(coord_data[1])*50;
                        double z = Convert.ToDouble(coord_data[2])*50;
                        int index = i - 11;
                        //double[] pt = new double[] { x, y, z };
                        pts[index, 0] = x;
                        pts[index, 1] = y;
                        pts[index, 2] = z;
                        Gl.Vertex(x, y, z);
                        
                        // 获取每个点的坐标
                        Console.WriteLine(i + " x: " + x + ",y: " + y + ",z: " + z);

                        max_x = Math.Max(x, max_x);
                        max_y = Math.Max(y, max_y);
                        max_z = Math.Max(z, max_z);

                        min_x = Math.Min(x, min_x);
                        min_y = Math.Min(y, min_y);
                        min_z = Math.Min(z, min_z);
                    }
                    //{
                    //}

                }
            }
            // for 找到所有点集合中xyz的最大值、最小值
            // 找最大距离
            radius = Math.Max(Math.Max(Math.Abs(max_x - min_x), Math.Abs(max_y - min_y)), Math.Abs(max_z - min_z));
            // 定义了初始看的角度
            upz = 1;
            rx = 0;
            ry = CV_PI * 45 / 180.0;
            // 根据最大最小计算均值
            nx = (max_x + min_x) / 2 * -1;
            ny = (max_y + min_y) / 2 * -1;
            nz = (max_z + min_z) / 2 * -1;
            Gl.End();
            Gl.EndList();
            Gl.PopMatrix();

            Console.WriteLine("get points finish");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Console.WriteLine("fsdfsa");
            OpenGL gl = openGLControl1.OpenGL;
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

            gl.LoadIdentity();
            //gl.Rotate(rotation, 0.0f, 1.0f, 0.0f);
            drawGrid(ref gl);
            DrawPCDFile(ref gl);
        }

        private void openGLControl1_MouseMove(object sender, MouseEventArgs e)
        {
            // e.Y:鼠标当前的y坐标
            // my :鼠标上一次使用时候的y坐标
            // 计算当前y最新弧度值
            if (btn_on_move)
            {
                ry = ry + (e.Y - my) * (CV_PI * 0.2 / 180.0);
                if (Math.Cos(ry) > 0)
                // ry 在[-PI/2, PI/2]内
                {
                    upz = 1;//Z的方向
                    rx = rx - (e.X - mx) * (CV_PI * 0.2 / 180.0);
                    Console.WriteLine("z=1 , rx:" + rx + " ,ry:" + ry);
                }
                else
                {
                    upz = -1;
                    rx = rx + (e.X - mx) * (CV_PI * 0.2 / 180.0);
                    Console.WriteLine("z=-1 , rx:" + rx + " ,ry:" + ry);
                };
                mx = e.X; my = e.Y;
                UpdateDrawData();
            }
            else if (btn_change_view)
            {
                cy = cy + (e.Y - my) * (CV_PI * 0.2 / 180.0);
                if (Math.Cos(cy) > 0)
                // ry 在[-PI/2, PI/2]内
                {
                    upz = 1;//Z的方向
                    cx = cx - (e.X - mx) * (CV_PI * 0.2 / 180.0);
                    Console.WriteLine("z=1 , cx:" + cx + " ,cy:" + cy);
                }
                else
                {
                    upz = -1;
                    cx = cx + (e.X - mx) * (CV_PI * 0.2 / 180.0);
                    Console.WriteLine("z=-1 , cx:" + cx + " ,cy:" + cy);
                };
                mx = e.X; my = e.Y;
                UpdateDrawData();
            }
            Console.WriteLine("rxy ==> rx: " + rx + ", ry: " + ry);
            Console.WriteLine("MouseDown ==> e.X: " + e.X + ", e.Y: " + e.Y);

        }

        private void openGLControl1_MouseDown(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                btn_on_move = true;
                mx = e.X; my = e.Y;
            }
            else if(e.Button == MouseButtons.Right)
            {
                btn_change_view = true;
                mx = e.X; my = e.Y;
            }
            
        }

        private void openGLControl1_MouseUp(object sender, MouseEventArgs e)
        {
            btn_on_move = false;
            btn_change_view = false;
        }
    }
}
