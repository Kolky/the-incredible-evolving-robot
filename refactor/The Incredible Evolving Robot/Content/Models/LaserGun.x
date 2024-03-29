xof 0303txt 0032
template KeyValuePair {
 <26e6b1c3-3d4d-4a1d-a437-b33668ffa1c2>
 STRING key;
 STRING value;
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

template ObjectMatrixComment {
 <95a48e28-7ef4-4419-a16a-ba9dbdf0d2bc>
 Matrix4x4 objectMatrix;
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


KeyValuePair {
 "Date";
 "2007-11-28 14:09:38";
}

KeyValuePair {
 "File";
 "";
}

KeyValuePair {
 "User";
 "maarten";
}

KeyValuePair {
 "CoreTime";
 "0";
}

Frame Pyramid01 {
 

 FrameTransformMatrix relative {
  1.000000,0.000000,0.000000,0.000000,0.000000,0.000000,-1.000000,0.000000,0.000000,1.000000,0.000000,0.000000,0.000000,0.000000,0.000000,1.000000;;
 }

 ObjectMatrixComment object {
  1.000000,0.000000,0.000000,0.000000,0.000000,1.000000,0.000000,0.000000,0.000000,0.000000,1.000000,0.000000,0.000000,0.000000,-9.000000,1.000000;;
 }

 Mesh mesh_Pyramid01 {
  17;
  0.000000;0.000000;-0.500000;,
  -0.450000;-0.450000;-9.000000;,
  0.450000;-0.450000;-9.000000;,
  0.000000;0.000000;-0.500000;,
  0.450000;-0.450000;-9.000000;,
  0.450000;0.450000;-9.000000;,
  0.000000;0.000000;-0.500000;,
  0.450000;0.450000;-9.000000;,
  -0.450000;0.450000;-9.000000;,
  0.000000;0.000000;-0.500000;,
  -0.450000;0.450000;-9.000000;,
  -0.450000;-0.450000;-9.000000;,
  -0.450000;-0.450000;-9.000000;,
  0.000000;0.000000;-9.000000;,
  0.450000;-0.450000;-9.000000;,
  0.450000;0.450000;-9.000000;,
  -0.450000;0.450000;-9.000000;;
  8;
  3;0,1,2;,
  3;3,4,5;,
  3;6,7,8;,
  3;9,10,11;,
  3;12,13,14;,
  3;14,13,15;,
  3;15,13,16;,
  3;16,13,12;;

  MeshNormals normals {
   17;
   0.000000;-0.998601;0.052867;,
   0.000000;-0.998602;0.052867;,
   0.000000;-0.998602;0.052867;,
   0.998601;0.000000;0.052867;,
   0.998602;0.000000;0.052867;,
   0.998602;0.000000;0.052867;,
   0.000000;0.998601;0.052867;,
   0.000000;0.998602;0.052867;,
   0.000000;0.998602;0.052867;,
   -0.998601;0.000000;0.052867;,
   -0.998602;0.000000;0.052867;,
   -0.998602;0.000000;0.052867;,
   0.000000;0.000000;-1.000000;,
   0.000000;0.000000;-1.000000;,
   0.000000;0.000000;-1.000000;,
   0.000000;0.000000;-1.000000;,
   0.000000;0.000000;-1.000000;;
   8;
   3;0,1,2;,
   3;3,4,5;,
   3;6,7,8;,
   3;9,10,11;,
   3;12,13,14;,
   3;14,13,15;,
   3;15,13,16;,
   3;16,13,12;;
  }

  MeshTextureCoords tc0 {
   17;
   0.500000;0.000000;,
   0.000000;1.000000;,
   1.000000;1.000000;,
   0.500000;0.000000;,
   0.000000;1.000000;,
   1.000000;1.000000;,
   0.500000;0.000000;,
   0.000000;1.000000;,
   1.000000;1.000000;,
   0.500000;0.000000;,
   0.000000;1.000000;,
   1.000000;1.000000;,
   0.000000;0.000000;,
   0.500000;0.500000;,
   1.000000;0.000000;,
   1.000000;1.000000;,
   0.000000;1.000000;;
  }
 }
}