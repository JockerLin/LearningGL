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
        public Form1()
        {
            InitializeComponent();
        }

        private void openGLControl1_OpenGLDraw(object sender, PaintEventArgs e)
        {
            SharpGL.OpenGL gl = this.openGLControl1.OpenGL;
            //清除深度缓存 
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

            //重置当前指定的矩阵为单位矩阵,将当前的用户坐标系的原点移到了屏幕中心
            gl.LoadIdentity();

            //坐标轴变换位置到(-1.5,0,-6)
            gl.Translate(-1.5f, 0f, -6f);

            gl.Begin(OpenGL.GL_TRIANGLES);
            {
                //顶点 
                gl.Vertex(0.0f, 1.0f, 0.0f);
                //左端点      
                gl.Vertex(-1.0f, -1.0f, 0.0f);
                //右端点       
                gl.Vertex(1.0f, -1.0f, 0.0f);
            }
            gl.End();

            //把当前坐标系右移3个单位，注意此时是相对上面(-1.5,0,-6)点定位 
            gl.Translate(3f, 0f, 0f);

            gl.Begin(OpenGL.GL_QUADS);
            {
                gl.Vertex(-1.0f, 1.0f, 0.0f);
                gl.Vertex(1.0f, 1.0f, 0.0f);
                gl.Vertex(1.0f, -1.0f, 0.0f);
                gl.Vertex(-1.0f, -1.0f, 0.0f);
            }
            gl.End();
            gl.Flush();   //强制刷新
        }

        private void openGLControl1_OpenGLInitialized(object sender, EventArgs e)
        {
            OpenGL gl = openGLControl1.OpenGL;
            gl.ClearColor(0, 0, 0, 0);
        }

        private void openGLControl1_Resized(object sender, EventArgs e)
        {

        }
    }
}
