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

//显示栅格球体
namespace ShowBall
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        void drawGrid(OpenGL gl)
        {
            //绘制栅格线过程
            gl.PushAttrib(OpenGL.GL_CURRENT_BIT);  //保存当前属性
            gl.PushMatrix();                        //压入堆栈
            gl.Translate(0f, 0f, 0f);
            gl.Color(0f, 0f, 1f);

            //在X,Z平面上绘制网格
            for (float i = -50; i <= 50; i += 1)
            {
                //绘制线
                gl.Begin(OpenGL.GL_LINES);
                //X轴方向
                //x y z
                gl.Vertex(-50f, 0f, i);
                gl.Vertex(50f, 0f, i);
                //Z轴方向
                gl.Vertex(i, 0f, -50f);
                gl.Vertex(i, 0f, 50f);
                gl.End();
            }
            gl.PopMatrix();
            gl.PopAttrib();
        }

        void drawSphere(OpenGL gl)
        {
            //画二次曲面球体绘制过程
            gl.PushMatrix();
            gl.Translate(2f, 1f, 2f);

            //绘制二次曲面
            var sphere = gl.NewQuadric();
            //设置二次却面绘制风格。gluQuadricDrawStyle。一般都是选用GLU_FILL风格，采用多边形来模拟
            gl.QuadricDrawStyle(sphere, OpenGL.GLU_LINE);
            //设置法线风格。gluQuadricNormals。一般都是使用GLU_SMOOTH风格，对每个顶点都计算法线向量，是默认方式
            gl.QuadricNormals(sphere, OpenGL.GLU_SMOOTH);
            //设置二次曲面的绘制方向。gluQuadricOrientation。一般使用GLU_OUTSIDE, 按照所有的法线都指向外面的方式绘制。是默认方式
            gl.QuadricOrientation(sphere, (int)OpenGL.GLU_OUTSIDE);
            //设置纹理。gluQuadricTexture。设置是否自动计算纹理。默认是GLU_FALSE。当需要使用纹理时应修改为GLU_TRUE.
            gl.QuadricTexture(sphere, (int)OpenGL.GLU_FALSE);

            gl.Sphere(sphere, 3f, 20, 10);
            gl.DeleteQuadric(sphere);
            gl.PopMatrix();
        }

        private void openGLControl1_OpenGLDraw(object sender, PaintEventArgs e)
        {
            OpenGL gl = openGLControl1.OpenGL;
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            gl.LoadIdentity();

            double[] eqn = new double[4] { 1f, 0f, 0f, 0f };

            gl.Color(1.0, 1.0, 1.0);

            gl.PushMatrix();
            {
                gl.Translate(-2, -2, -3);
                gl.Rotate(-90.0, 1.0, 0.0, 0.0);
                drawSphere(gl);
            }
            gl.PopMatrix();

            gl.PushMatrix();
            {
                //gl.ClipPlane(OpenGL.GL_CLIP_PLANE0, eqn);
                //gl.Enable(OpenGL.GL_CLIP_PLANE0);
                drawGrid(gl);
            }
            gl.PopMatrix();

            gl.Flush();
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
            gl.Perspective(60.0f, (double)Width / (double)Height, 1, 100.0);
            // 3 5 10 为视点
            gl.LookAt(3, 5, 10, 0, 0, 0, 0, 1, 0);
            gl.MatrixMode(OpenGL.GL_MODELVIEW);
        }
    }
}
