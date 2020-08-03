using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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

        public Form1()
        {
            InitializeComponent();
            angle = 0.0f;                    // 设置初始角度为0
            legAngle[0] = legAngle[1] = 0.0f;
            armAngle[0] = armAngle[1] = 0.0f;
        }

        void drawGrid(OpenGL gl)
        {
            //绘制过程
            gl.PushAttrib(OpenGL.GL_CURRENT_BIT);  //保存当前属性
            gl.PushMatrix();                        //压入堆栈
            gl.Translate(0f, -20f, 0f);
            gl.Color(0f, 0f, 1f);

            //在X,Z平面上绘制网格
            for (float i = -50; i <= 50; i += 1)
            {
                //绘制线
                gl.Begin(OpenGL.GL_LINES);
                {
                    if (i == 0)
                        gl.Color(0f, 1f, 0f);
                    else
                        gl.Color(0f, 0f, 1f);

                    //X轴方向 连线(-50, 0,30)与(50, 0, 30)
                    gl.Vertex(-50f, 0f, i);
                    gl.Vertex(50f, 0f, i);
                    //Z轴方向
                    //gl.Vertex(i, 0f, -40f);
                    //gl.Vertex(i, 0f, 40f);

                }
                gl.End();
            }
            gl.PopMatrix();
            gl.PopAttrib();
        }

        private void openGLControl1_OpenGLDraw(object sender, PaintEventArgs e)
        {
            // 实际运行是每一帧都会进这个函数
            OpenGL gl = openGLControl1.OpenGL;
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

            gl.LoadIdentity();
            gl.Rotate(rotation, 0.0f, 1.0f, 0.0f);

            drawGrid(gl);
            DrawRobot(ref gl, 0, 0, 0);
            //rtest(ref gl, 0, 0, 0);
            rotation += 1.0f;
        }

        private void openGLControl1_OpenGLInitialized(object sender, EventArgs e)
        {
            OpenGL gl = openGLControl1.OpenGL;
            gl.ClearColor(0, 0, 0, 0);
        }

        private void openGLControl1_Resized(object sender, EventArgs e)
        {
            OpenGL gl = openGLControl1.OpenGL;

            gl.MatrixMode(OpenGL.GL_PROJECTION);

            gl.LoadIdentity();
            gl.Perspective(60.0f, (double)Width / (double)Height, 0.01, 100.0);
            gl.LookAt(-5, 5, 15, 0, 0, 0, 0, 1, 0);
            gl.MatrixMode(OpenGL.GL_MODELVIEW);
        }

        public void rtest(ref OpenGL gl, float xPos, float yPos, float zPos)
        {
            // glPushMatrix()就是“记住自己在哪”，glPopMatrix()就是“返回自己原来所在地”
            Console.WriteLine("in ");
            gl.PushMatrix();
            {
                gl.Color(1f, 0f, 0f);
                gl.Translate(xPos, yPos, zPos);
                // x方向为两倍缩放
                gl.Scale(2f, 1f, 1f);
                // x方向旋转45°
                gl.Rotate(45, 1f, 0f, 0f);
                DrawCube(ref gl, 0, 0, 0, true);
            }
            gl.PopMatrix();

            gl.PushMatrix();
            {
                gl.Color(0f, 1f, 0f);
                gl.Translate(xPos - 2f, yPos, zPos);
                DrawCube(ref gl, 0, 0, 0, true);
            }
            gl.PopMatrix();
        }

        internal void DrawCube(ref OpenGL Gl, float xPos, float yPos, float zPos, bool isLine)
        {
            Gl.PushMatrix();
            Gl.Translate(xPos, yPos, zPos);
            if (isLine)
                Gl.Begin(OpenGL.GL_LINE_STRIP);
            else
                Gl.Begin(OpenGL.GL_POLYGON);

            // 顶面
            Gl.Vertex(0.0f, 0.0f, 0.0f);
            Gl.Vertex(0.0f, 0.0f, -1.0f);
            Gl.Vertex(-1.0f, 0.0f, -1.0f);
            Gl.Vertex(-1.0f, 0.0f, 0.0f);

            // 前面
            Gl.Vertex(0.0f, 0.0f, 0.0f);
            Gl.Vertex(-1.0f, 0.0f, 0.0f);
            Gl.Vertex(-1.0f, -1.0f, 0.0f);
            Gl.Vertex(0.0f, -1.0f, 0.0f);

            // 右面
            Gl.Vertex(0.0f, 0.0f, 0.0f);
            Gl.Vertex(0.0f, -1.0f, 0.0f);
            Gl.Vertex(0.0f, -1.0f, -1.0f);
            Gl.Vertex(0.0f, 0.0f, -1.0f);

            // 左面
            Gl.Vertex(-1.0f, 0.0f, 0.0f);
            Gl.Vertex(-1.0f, 0.0f, -1.0f);
            Gl.Vertex(-1.0f, -1.0f, -1.0f);
            Gl.Vertex(-1.0f, -1.0f, 0.0f);

            // 底面
            Gl.Vertex(0.0f, 0.0f, 0.0f);
            Gl.Vertex(0.0f, -1.0f, -1.0f);
            Gl.Vertex(-1.0f, -1.0f, -1.0f);
            Gl.Vertex(-1.0f, -1.0f, 0.0f);


            // 后面
            Gl.Vertex(0.0f, 0.0f, 0.0f);
            Gl.Vertex(-1.0f, 0.0f, -1.0f);
            Gl.Vertex(-1.0f, -1.0f, -1.0f);
            Gl.Vertex(0.0f, -1.0f, -1.0f);

            // 8点法
            //Gl.Vertex(0.0f, 0.0f, 0.0f);
            //Gl.Vertex(-1.0f, 0.0f, 0.0f);
            //Gl.Vertex(-1.0f, -1.0f, 0.0f);
            //Gl.Vertex(0.0f, -1.0f, 0.0f);
            //Gl.Vertex(0.0f, 0.0f, -1.0f);
            //Gl.Vertex(-1.0f, 0.0f, -1.0f);
            //Gl.Vertex(-1.0f, -1.0f, -1.0f);
            //Gl.Vertex(0.0f, -1.0f, -1.0f);

            Gl.End();
            Gl.PopMatrix();
        }

        public void DrawRobot(ref OpenGL Gl, float xPos, float yPos, float zPos)
        {
            Gl.PushMatrix();
            {
                Gl.Translate(xPos, yPos, zPos);

                ///绘制各个部分
                //Gl.LoadIdentity();
                DrawHead(ref Gl, 1f, 2f, 0f);     // 绘制头部  2*2*2
                DrawTorso(ref Gl, 1.5f, 0.0f, 0.0f); //躯干,  3*5*2

                Gl.PushMatrix();
                {
                    //如果胳膊正在向前运动，则递增角度，否则递减角度
                    if (arm1)
                        armAngle[0] = armAngle[0] + 1f;
                    else
                        armAngle[0] = armAngle[0] - 1f;

                    ///如果胳膊达到其最大角度则改变其状态
                    if (armAngle[0] >= 15.0f)
                        arm1 = false;
                    if (armAngle[0] <= -15.0f)
                        arm1 = true;

                    //平移并旋转后绘制胳膊
                    Gl.Translate(0.0f, -0.5f, 0.0f);
                    Gl.Rotate(armAngle[0], 1.0f, 0.0f, 0.0f);
                    DrawArm(ref Gl, 2.5f, 0.0f, -0.5f);  //胳膊1, 1*4*1
                }
                Gl.PopMatrix();

                Gl.PushMatrix();
                {
                    if (arm2)
                        armAngle[1] = armAngle[1] + 1f;
                    else
                        armAngle[1] = armAngle[1] - 1f;


                    if (armAngle[1] >= 15.0f)
                        arm2 = false;
                    if (armAngle[1] <= -15.0f)
                        arm2 = true;


                    Gl.Translate(0.0f, -0.5f, 0.0f);
                    Gl.Rotate(armAngle[1], 1.0f, 0.0f, 0.0f);
                    DrawArm(ref Gl, -1.5f, 0.0f, -0.5f); //胳膊2, 1*4*1
                }
                Gl.PopMatrix();

                Gl.PushMatrix();
                {
                    ///如果腿正在向前运动，则递增角度，否则递减角度
                    if (leg1)
                        legAngle[0] = legAngle[0] + 1f;
                    else
                        legAngle[0] = legAngle[0] - 1f;

                    ///如果腿达到其最大角度则改变其状态
                    if (legAngle[0] >= 15.0f)
                        leg1 = false;
                    if (legAngle[0] <= -15.0f)
                        leg1 = true;

                    ///平移并旋转后绘制胳膊
                    Gl.Translate(0.0f, -0.5f, 0.0f);
                    Gl.Rotate(legAngle[0], 1.0f, 0.0f, 0.0f);
                    DrawLeg(ref Gl, -0.5f, -5.0f, -0.5f); //腿部1,1*5*1
                }
                Gl.PopMatrix();

                Gl.PushMatrix();
                {
                    if (leg2)
                        legAngle[1] = legAngle[1] + 1f;
                    else
                        legAngle[1] = legAngle[1] - 1f;

                    if (legAngle[1] >= 15.0f)
                        leg2 = false;
                    if (legAngle[1] <= -15.0f)
                        leg2 = true;

                    Gl.Translate(0.0f, -0.5f, 0.0f);
                    Gl.Rotate(legAngle[1], 1.0f, 0.0f, 0.0f);
                    DrawLeg(ref Gl, 1.5f, -5.0f, -0.5f); //腿部2, 1*5*1
                }
                Gl.PopMatrix();
            }
            Gl.PopMatrix();
        }

        // 绘制一个手臂
        void DrawArm(ref OpenGL Gl, float xPos, float yPos, float zPos)
        {
            Gl.PushMatrix();
            Gl.Color(1.0f, 0.0f, 0.0f);    // 红色
            Gl.Translate(xPos, yPos, zPos);
            Gl.Scale(1.0f, 4.0f, 1.0f);        // 手臂是1x4x1的立方体
            DrawCube(ref Gl, 0.0f, 0.0f, 0.0f, false);
            Gl.PopMatrix();
        }

        // 绘制一条腿
        void DrawLeg(ref OpenGL Gl, float xPos, float yPos, float zPos)
        {
            Gl.PushMatrix();
            Gl.Color(1.0f, 1.0f, 0.0f);    // 黄色
            Gl.Translate(xPos, yPos, zPos);
            Gl.Scale(1.0f, 3.0f, 1.0f);        // 腿是1x5x1长方体
            DrawCube(ref Gl, 0.0f, 0.0f, 0.0f, false);
            Gl.PopMatrix();
        }

        // 绘制头部
        void DrawHead(ref OpenGL Gl, float xPos, float yPos, float zPos)
        {
            Gl.PushMatrix();
            Gl.Color(1.0f, 1.0f, 1.0f);    // 白色
            Gl.Translate(xPos, yPos, zPos);
            Gl.Scale(2.0f, 2.0f, 2.0f);        //头部是 2x2x2长方体
            DrawCube(ref Gl, 0.0f, 0.0f, 0.0f, false);
            Gl.PopMatrix();
        }

        // 绘制机器人的躯干
        void DrawTorso(ref OpenGL Gl, float xPos, float yPos, float zPos)
        {
            Gl.PushMatrix();
            Gl.Color(0.0f, 0.0f, 1.0f);     // 蓝色
            Gl.Translate(xPos, yPos, zPos);
            Gl.Scale(3.0f, 5.0f, 2.0f);         // 躯干是3x5x2的长方体
            DrawCube(ref Gl, 0.0f, 0.0f, 0.0f, false);
            Gl.PopMatrix();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Console.WriteLine("fsdfsa");

        }
    }
}
