//#define VBO
//#define VAO
//#define EBO
#define TEXTURE

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;


namespace HelloOpenTK
{
    class Game : GameWindow
    {
        int VertexBufferObject;
#if VBO
        float[] vertices = {
            -0.5f, -0.5f, 0.0f, //Bottom-left vertex
             0.5f, -0.5f, 0.0f, //Bottom-right vertex
             0.0f,  0.5f, 0.0f  //Top vertex
        };
#endif
#if VAO
        float[] vertices = {
            -0.5f, -0.5f, 0.0f, //Bottom-left vertex
             0.5f, -0.5f, 0.0f, //Bottom-right vertex
             0.0f,  0.5f, 0.0f  //Top vertex
        };
        int VertexArrayObject;
#endif
#if EBO
        float[] vertices = {
             0.5f,  0.5f, 0.0f,  // top right
             0.5f, -0.5f, 0.0f,  // bottom right
            -0.5f, -0.5f, 0.0f,  // bottom left
            -0.5f,  0.5f, 0.0f   // top left
        };
        uint[] indices = {  // note that we start from 0!
            0, 1, 3,   // first triangle
            1, 2, 3    // second triangle
        };
        int ElementBufferObject;
#endif

#if TEXTURE
        int ElementBufferObject;
        float[] vertices =
        {
            //Position          Texture coordinates
                1f,  1f, 0.0f, 1.0f, 1.0f, // top right
                1f, -1f, 0.0f, 1.0f, 0.0f, // bottom right
            -1f, -1f, 0.0f, 0.0f, 0.0f, // bottom left
            -1f,  1f, 0.0f, 0.0f, 1.0f  // top left
        };
        uint[] indices = {  // note that we start from 0!
            0, 1, 3,   // first triangle
            1, 2, 3    // second triangle
        };
        float[] texCoords = {
            0.0f, 0.0f,  // lower-left corner  
            1.0f, 0.0f,  // lower-right corner
            0.5f, 1.0f   // top-center corner
        };
#endif

        Shader shader;
        Texture texture;
        Texture texture2;
        public Game(int width, int height, string title) : base(width, height, GraphicsMode.Default, title)
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            //Code goes here
            // 建顶点着色器对象

#if VBO
            shader = new Shader("shader.vert", "shader.frag");
            shader.Use();
            // -------------------顶点缓存对象VBO--------------------------------
            // 生成缓存对象
            VertexBufferObject = GL.GenBuffer();
            // 绑定缓存对象
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            // 填入数据--将顶点数组复制到OpenGL要使用的缓冲区中
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            // 连接顶点属性
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            // -------------------------------------------------------
#endif
#if VAO
            shader = new Shader("shader.vert", "shader.frag");
            shader.Use();
            //-------------------顶点数组对象VAO------------------------------
            // 生成缓存对象
            VertexBufferObject = GL.GenBuffer();
            // 生成顶点数组对象
            VertexArrayObject = GL.GenVertexArray();
            // 绑定顶点数组对象
            GL.BindVertexArray(VertexArrayObject);
            // 绑定缓存对象
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            // 设置顶点属性
             GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
#endif

#if EBO
            shader = new Shader("shader.vert", "shader.frag");
            shader.Use();
            // 生成VBO
            VertexBufferObject = GL.GenBuffer();
            // 绑定缓存对象
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            // 填入数据--将顶点数组复制到OpenGL要使用的缓冲区中
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            // 生成缓存对象
            ElementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
#endif

#if TEXTURE
            shader = new Shader("shader.vert", "shaderTexture.frag");
            shader.Use();
            //创建纹理
            texture = new Texture("timg.jpg");
            texture2 = new Texture("texture2.jpg");

            // 生成VBO
            VertexBufferObject = GL.GenBuffer();
            // 绑定缓存对象
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            // 填入数据--将顶点数组复制到OpenGL要使用的缓冲区中
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            // 生成缓存对象
            ElementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            //GL.EnableVertexAttribArray(0);

            shader.SetInt("texture1", 0);
            shader.SetInt("texture2", 1);
            //纹理映射

            //纹理过滤
            float[] borderColor = { 1.0f, 1.0f, 0.0f, 1.0f };
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBorderColor, borderColor);
            //int texCoordLocation = shader2.GetAttribLocation("aTexCoord");
           GL.EnableVertexAttribArray(0);
           GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
            // 连接顶点属性
            GL.EnableVertexAttribArray(0);
#endif


            base.OnLoad(e);
        }
        protected override void OnUnload(EventArgs e)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
#if VBO
            GL.DeleteBuffer(VertexBufferObject);
#endif
            shader.Dispose();
            base.OnUnload(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            shader.Use();
#if VBO
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
#endif

#if VAO
            GL.BindVertexArray(VertexArrayObject);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
#endif

#if EBO
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
#endif

#if TEXTURE
            texture.Use();
            texture2.Use();
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
#endif
            Context.SwapBuffers();
            base.OnRenderFrame(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            //Get the state of the keyboard this frame
            KeyboardState input = Keyboard.GetState();

            if (input.IsKeyDown(Key.Escape))
            {
                Exit();
            }

            base.OnUpdateFrame(e);
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            base.OnResize(e);
        }
    }
}
