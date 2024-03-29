xof 0303txt 0032
template ColorRGBA {
 <35ff44e0-6c7c-11cf-8f52-0040333594a3>
 FLOAT red;
 FLOAT green;
 FLOAT blue;
 FLOAT alpha;
}

template ColorRGB {
 <d3e16e81-7835-11cf-8f52-0040333594a3>
 FLOAT red;
 FLOAT green;
 FLOAT blue;
}

template Material {
 <3d82ab4d-62da-11cf-ab39-0020af71e433>
 ColorRGBA faceColor;
 FLOAT power;
 ColorRGB specularColor;
 ColorRGB emissiveColor;
 [...]
}

template Frame {
 <3d82ab46-62da-11cf-ab39-0020af71e433>
 [...]
}

template Matrix4x4 {
 <f6f23f45-7686-11cf-8f52-0040333594a3>
 array FLOAT matrix[16];
}

template FrameTransformMatrix {
 <f6f23f41-7686-11cf-8f52-0040333594a3>
 Matrix4x4 frameMatrix;
}

template Vector {
 <3d82ab5e-62da-11cf-ab39-0020af71e433>
 FLOAT x;
 FLOAT y;
 FLOAT z;
}

template MeshFace {
 <3d82ab5f-62da-11cf-ab39-0020af71e433>
 DWORD nFaceVertexIndices;
 array DWORD faceVertexIndices[nFaceVertexIndices];
}

template Mesh {
 <3d82ab44-62da-11cf-ab39-0020af71e433>
 DWORD nVertices;
 array Vector vertices[nVertices];
 DWORD nFaces;
 array MeshFace faces[nFaces];
 [...]
}

template MeshNormals {
 <f6f23f43-7686-11cf-8f52-0040333594a3>
 DWORD nNormals;
 array Vector normals[nNormals];
 DWORD nFaceNormals;
 array MeshFace faceNormals[nFaceNormals];
}

template MeshMaterialList {
 <f6f23f42-7686-11cf-8f52-0040333594a3>
 DWORD nMaterials;
 DWORD nFaceIndexes;
 array DWORD faceIndexes[nFaceIndexes];
 [Material <3d82ab4d-62da-11cf-ab39-0020af71e433>]
}

template Coords2d {
 <f6f23f44-7686-11cf-8f52-0040333594a3>
 FLOAT u;
 FLOAT v;
}

template MeshTextureCoords {
 <f6f23f40-7686-11cf-8f52-0040333594a3>
 DWORD nTextureCoords;
 array Coords2d textureCoords[nTextureCoords];
}


Material PDX01_-_Default {
 0.874510;0.772549;0.196078;1.000000;;
 3.200000;
 0.000000;0.000000;0.000000;;
 0.000000;0.000000;0.000000;;
}

Frame laser {
 

 FrameTransformMatrix {
  0.871512,0.000000,0.000000,0.000000,0.000000,-1.000000,0.000000,0.000000,0.000000,-0.000000,-0.871512,0.000000,0.000000,0.000000,0.000000,1.000000;;
 }

 Frame {
  

  FrameTransformMatrix {
   1.000000,0.000000,0.000000,0.000000,0.000000,1.000000,0.000000,0.000000,0.000000,0.000000,1.000000,0.000000,0.000000,0.000000,0.000000,1.000000;;
  }

  Mesh  {
   15;
   0.000000;0.000000;0.000000;,
   1.000000;0.000000;0.000000;,
   -0.000000;0.000000;1.000000;,
   -1.000000;0.000000;-0.000000;,
   0.000000;0.000000;-1.000000;,
   0.422726;3.000000;-0.000000;,
   -0.000000;3.000000;0.422726;,
   -0.422726;3.000000;-0.000000;,
   -0.000000;3.000000;-0.422726;,
   -0.000000;3.000000;-0.000000;,
   1.000000;0.000000;0.000000;,
   0.422726;3.000000;-0.000000;,
   1.000000;0.000000;0.000000;,
   0.422726;3.000000;-0.000000;,
   0.422726;3.000000;-0.000000;;
   16;
   3;0,1,2;,
   3;0,2,3;,
   3;0,3,4;,
   3;0,4,10;,
   3;1,5,6;,
   3;1,6,2;,
   3;2,6,7;,
   3;2,7,3;,
   3;3,7,8;,
   3;3,8,4;,
   3;4,8,11;,
   3;4,13,12;,
   3;9,6,5;,
   3;9,7,6;,
   3;9,8,7;,
   3;9,14,8;;

   MeshNormals  {
    14;
    0.000000;-1.000000;-0.000000;,
    0.000000;-1.000000;0.000000;,
    0.000000;-1.000000;-0.000000;,
    0.000000;-1.000000;-0.000000;,
    0.000000;-1.000000;-0.000000;,
    0.700651;0.134822;0.700651;,
    0.311087;0.179582;0.933261;,
    -0.311087;0.179582;0.933261;,
    -0.933261;0.179582;0.311087;,
    -0.933261;0.179582;-0.311087;,
    -0.311087;0.179582;-0.933261;,
    0.311087;0.179582;-0.933261;,
    0.700651;0.134822;-0.700651;,
    0.000000;1.000000;0.000000;;
    16;
    3;0,1,1;,
    3;0,1,2;,
    3;0,2,3;,
    3;0,3,4;,
    3;5,5,6;,
    3;5,6,7;,
    3;7,6,8;,
    3;7,8,9;,
    3;9,8,10;,
    3;9,10,11;,
    3;11,10,12;,
    3;11,12,12;,
    3;13,13,13;,
    3;13,13,13;,
    3;13,13,13;,
    3;13,13,13;;
   }

   MeshMaterialList  {
    1;
    16;
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0;
    { PDX01_-_Default }
   }

   MeshTextureCoords  {
    15;
    0.500000;1.000000;,
    -0.250000;1.000000;,
    0.000000;1.000000;,
    0.250000;1.000000;,
    0.500000;1.000000;,
    -0.250000;0.000000;,
    0.000000;0.000000;,
    0.250000;0.000000;,
    0.500000;0.000000;,
    0.500000;0.000000;,
    0.750000;1.000000;,
    0.750000;0.000000;,
    0.750000;1.000000;,
    0.750000;0.000000;,
    0.750000;0.000000;;
   }
  }
 }
}