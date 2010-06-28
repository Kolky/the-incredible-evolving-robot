xof 0303txt 0032
template FVFData {
 <b6e70a0e-8ef9-4e83-94ad-ecc8b0c04897>
 DWORD dwFVF;
 DWORD nDWords;
 array DWORD data[nDWords];
}

template EffectInstance {
 <e331f7e4-0559-4cc2-8e99-1cec1657928f>
 STRING EffectFilename;
 [...]
}

template EffectParamFloats {
 <3014b9a0-62f5-478c-9b86-e4ac9f4e418b>
 STRING ParamName;
 DWORD nFloats;
 array FLOAT Floats[nFloats];
}

template EffectParamString {
 <1dbc4c88-94c1-46ee-9076-2c28818c9481>
 STRING ParamName;
 STRING Value;
}

template EffectParamDWord {
 <e13963bc-ae51-4c5d-b00f-cfa3a9d97ce5>
 STRING ParamName;
 DWORD Value;
}


Frame Sphere01 {
 

 FrameTransformMatrix {
  1.000000,0.000000,0.000000,0.000000,0.000000,1.000000,0.000000,0.000000,0.000000,0.000000,1.000000,0.000000,0.000000,0.000000,0.000000,1.000000;;
 }

 Mesh  {
  223;
  0.000000;0.000000;2.000000;,
  -0.000000;0.765367;1.847759;,
  -0.292893;0.707107;1.847759;,
  -0.541196;0.541196;1.847759;,
  -0.707107;0.292893;1.847759;,
  -0.765367;0.000000;1.847759;,
  -0.707107;-0.292893;1.847759;,
  -0.541196;-0.541196;1.847759;,
  -0.292893;-0.707107;1.847759;,
  0.000000;-0.765367;1.847759;,
  0.292893;-0.707107;1.847759;,
  0.541196;-0.541196;1.847759;,
  0.707107;-0.292893;1.847759;,
  0.765367;0.000000;1.847759;,
  0.707107;0.292894;1.847759;,
  0.541196;0.541197;1.847759;,
  0.292892;0.707107;1.847759;,
  -0.000000;1.414214;1.414214;,
  -0.541196;1.306563;1.414214;,
  -1.000000;1.000000;1.414214;,
  -1.306563;0.541196;1.414214;,
  -1.414214;0.000000;1.414214;,
  -1.306563;-0.541196;1.414214;,
  -1.000000;-1.000000;1.414214;,
  -0.541196;-1.306563;1.414214;,
  0.000000;-1.414214;1.414214;,
  0.541196;-1.306563;1.414214;,
  1.000000;-1.000000;1.414214;,
  1.306563;-0.541195;1.414214;,
  1.414214;0.000001;1.414214;,
  1.306563;0.541197;1.414214;,
  0.999999;1.000001;1.414214;,
  0.541195;1.306564;1.414214;,
  -0.000000;1.847759;0.765367;,
  -0.707107;1.707107;0.765367;,
  -1.306563;1.306563;0.765367;,
  -1.707107;0.707107;0.765367;,
  -1.847759;0.000000;0.765367;,
  -1.707107;-0.707106;0.765367;,
  -1.306563;-1.306563;0.765367;,
  -0.707107;-1.707107;0.765367;,
  0.000000;-1.847759;0.765367;,
  0.707107;-1.707107;0.765367;,
  1.306563;-1.306563;0.765367;,
  1.707107;-0.707106;0.765367;,
  1.847759;0.000001;0.765367;,
  1.707106;0.707108;0.765367;,
  1.306562;1.306564;0.765367;,
  0.707105;1.707108;0.765367;,
  -0.000000;2.000000;-0.000000;,
  -0.765367;1.847759;-0.000000;,
  -1.414214;1.414214;-0.000000;,
  -1.847759;0.765367;-0.000000;,
  -2.000000;0.000000;-0.000000;,
  -1.847759;-0.765366;-0.000000;,
  -1.414214;-1.414213;-0.000000;,
  -0.765367;-1.847759;-0.000000;,
  0.000000;-2.000000;-0.000000;,
  0.765367;-1.847759;-0.000000;,
  1.414214;-1.414213;-0.000000;,
  1.847759;-0.765366;-0.000000;,
  2.000000;0.000001;-0.000000;,
  1.847758;0.765368;-0.000000;,
  1.414212;1.414215;-0.000000;,
  0.765365;1.847760;-0.000000;,
  -0.000000;1.847759;-0.765367;,
  -0.707107;1.707107;-0.765367;,
  -1.306563;1.306563;-0.765367;,
  -1.707107;0.707107;-0.765367;,
  -1.847759;0.000000;-0.765367;,
  -1.707107;-0.707106;-0.765367;,
  -1.306563;-1.306563;-0.765367;,
  -0.707107;-1.707107;-0.765367;,
  0.000000;-1.847759;-0.765367;,
  0.707107;-1.707107;-0.765367;,
  1.306563;-1.306563;-0.765367;,
  1.707107;-0.707106;-0.765367;,
  1.847759;0.000001;-0.765367;,
  1.707106;0.707108;-0.765367;,
  1.306562;1.306564;-0.765367;,
  0.707105;1.707108;-0.765367;,
  -0.000000;1.414214;-1.414214;,
  -0.541196;1.306563;-1.414214;,
  -1.000000;1.000000;-1.414214;,
  -1.306563;0.541196;-1.414214;,
  -1.414214;0.000000;-1.414214;,
  -1.306563;-0.541196;-1.414214;,
  -1.000000;-1.000000;-1.414214;,
  -0.541196;-1.306563;-1.414214;,
  0.000000;-1.414214;-1.414214;,
  0.541196;-1.306563;-1.414214;,
  1.000000;-1.000000;-1.414214;,
  1.306563;-0.541195;-1.414214;,
  1.414214;0.000001;-1.414214;,
  1.306563;0.541197;-1.414214;,
  0.999999;1.000001;-1.414214;,
  0.541195;1.306564;-1.414214;,
  -0.000000;0.765367;-1.847759;,
  -0.292893;0.707107;-1.847759;,
  -0.541196;0.541196;-1.847759;,
  -0.707107;0.292893;-1.847759;,
  -0.765367;0.000000;-1.847759;,
  -0.707107;-0.292893;-1.847759;,
  -0.541196;-0.541196;-1.847759;,
  -0.292893;-0.707107;-1.847759;,
  0.000000;-0.765367;-1.847759;,
  0.292893;-0.707107;-1.847759;,
  0.541196;-0.541196;-1.847759;,
  0.707107;-0.292893;-1.847759;,
  0.765367;0.000000;-1.847759;,
  0.707107;0.292894;-1.847759;,
  0.541196;0.541197;-1.847759;,
  0.292892;0.707107;-1.847759;,
  0.000000;0.000000;-2.000000;,
  3.000000;0.500000;0.500000;,
  3.000000;0.500000;-0.500000;,
  3.000000;-0.500000;0.500000;,
  3.000000;-0.500000;-0.500000;,
  1.898745;0.500000;-0.500000;,
  1.898745;0.500000;0.500000;,
  1.898745;-0.500000;0.500000;,
  1.898745;-0.500000;-0.500000;,
  0.500000;3.000000;-0.500000;,
  -0.500000;3.000000;-0.500000;,
  0.500000;3.000000;0.500000;,
  -0.500000;3.000000;0.500000;,
  -0.500000;1.899654;-0.500000;,
  0.500000;1.899654;-0.500000;,
  0.500000;1.899654;0.500000;,
  -0.500000;1.899654;0.500000;,
  -3.000000;0.500000;0.500000;,
  -3.000000;0.500000;-0.500000;,
  -3.000000;-0.500000;0.500000;,
  -3.000000;-0.500000;-0.500000;,
  -1.892464;0.500000;-0.500000;,
  -1.892464;0.500000;0.500000;,
  -1.892464;-0.500000;0.500000;,
  -1.892464;-0.500000;-0.500000;,
  0.500000;-3.000000;-0.500000;,
  -0.500000;-3.000000;-0.500000;,
  0.500000;-3.000000;0.500000;,
  -0.500000;-3.000000;0.500000;,
  -0.500000;-1.901340;-0.500000;,
  0.500000;-1.901340;-0.500000;,
  0.500000;-1.901340;0.500000;,
  -0.500000;-1.901340;0.500000;,
  0.000000;0.000000;2.000000;,
  0.000000;0.000000;2.000000;,
  0.000000;0.000000;2.000000;,
  0.000000;0.000000;2.000000;,
  0.000000;0.000000;2.000000;,
  0.000000;0.000000;2.000000;,
  0.000000;0.000000;2.000000;,
  0.000000;0.000000;2.000000;,
  0.000000;0.000000;2.000000;,
  0.000000;0.000000;2.000000;,
  0.000000;0.000000;2.000000;,
  0.000000;0.000000;2.000000;,
  0.000000;0.000000;2.000000;,
  0.000000;0.000000;2.000000;,
  0.000000;0.000000;2.000000;,
  -0.000000;0.765367;1.847759;,
  -0.000000;1.414214;1.414214;,
  -0.000000;1.847759;0.765367;,
  -0.000000;2.000000;-0.000000;,
  -0.000000;1.847759;-0.765367;,
  -0.000000;1.414214;-1.414214;,
  -0.000000;0.765367;-1.847759;,
  0.000000;0.000000;-2.000000;,
  0.000000;0.000000;-2.000000;,
  0.000000;0.000000;-2.000000;,
  0.000000;0.000000;-2.000000;,
  0.000000;0.000000;-2.000000;,
  0.000000;0.000000;-2.000000;,
  0.000000;0.000000;-2.000000;,
  0.000000;0.000000;-2.000000;,
  0.000000;0.000000;-2.000000;,
  0.000000;0.000000;-2.000000;,
  0.000000;0.000000;-2.000000;,
  0.000000;0.000000;-2.000000;,
  0.000000;0.000000;-2.000000;,
  0.000000;0.000000;-2.000000;,
  0.000000;0.000000;-2.000000;,
  3.000000;-0.500000;-0.500000;,
  3.000000;0.500000;-0.500000;,
  3.000000;0.500000;-0.500000;,
  1.898745;0.500000;-0.500000;,
  3.000000;-0.500000;0.500000;,
  1.898745;-0.500000;-0.500000;,
  3.000000;0.500000;0.500000;,
  1.898745;-0.500000;0.500000;,
  3.000000;-0.500000;0.500000;,
  1.898745;0.500000;0.500000;,
  -0.500000;3.000000;0.500000;,
  -0.500000;3.000000;-0.500000;,
  -0.500000;3.000000;-0.500000;,
  -0.500000;1.899654;-0.500000;,
  0.500000;3.000000;0.500000;,
  -0.500000;1.899654;0.500000;,
  0.500000;3.000000;-0.500000;,
  0.500000;3.000000;0.500000;,
  0.500000;1.899654;0.500000;,
  0.500000;1.899654;-0.500000;,
  -1.892464;0.500000;0.500000;,
  -3.000000;-0.500000;0.500000;,
  -1.892464;-0.500000;0.500000;,
  -3.000000;0.500000;0.500000;,
  -3.000000;-0.500000;0.500000;,
  -3.000000;0.500000;-0.500000;,
  -3.000000;0.500000;-0.500000;,
  -1.892464;-0.500000;-0.500000;,
  -3.000000;-0.500000;-0.500000;,
  -1.892464;0.500000;-0.500000;,
  0.500000;-1.901340;-0.500000;,
  0.500000;-1.901340;0.500000;,
  0.500000;-3.000000;0.500000;,
  0.500000;-3.000000;-0.500000;,
  0.500000;-3.000000;0.500000;,
  -0.500000;-3.000000;-0.500000;,
  -0.500000;-3.000000;-0.500000;,
  -0.500000;-3.000000;0.500000;,
  -0.500000;-1.901340;0.500000;,
  -0.500000;-1.901340;-0.500000;;
  264;
  3;0,1,2;,
  3;146,2,3;,
  3;147,3,4;,
  3;148,4,5;,
  3;149,5,6;,
  3;150,6,7;,
  3;151,7,8;,
  3;152,8,9;,
  3;153,9,10;,
  3;154,10,11;,
  3;155,11,12;,
  3;156,12,13;,
  3;157,13,14;,
  3;158,14,15;,
  3;159,15,16;,
  3;160,16,161;,
  3;1,17,18;,
  3;1,18,2;,
  3;2,18,19;,
  3;2,19,3;,
  3;3,19,20;,
  3;3,20,4;,
  3;4,20,21;,
  3;4,21,5;,
  3;5,21,22;,
  3;5,22,6;,
  3;6,22,23;,
  3;6,23,7;,
  3;7,23,24;,
  3;7,24,8;,
  3;8,24,25;,
  3;8,25,9;,
  3;9,25,26;,
  3;9,26,10;,
  3;10,26,27;,
  3;10,27,11;,
  3;11,27,28;,
  3;11,28,12;,
  3;12,28,29;,
  3;12,29,13;,
  3;13,29,30;,
  3;13,30,14;,
  3;14,30,31;,
  3;14,31,15;,
  3;15,31,32;,
  3;15,32,16;,
  3;16,32,162;,
  3;16,162,161;,
  3;17,33,34;,
  3;17,34,18;,
  3;18,34,35;,
  3;18,35,19;,
  3;19,35,36;,
  3;19,36,20;,
  3;20,36,37;,
  3;20,37,21;,
  3;21,37,38;,
  3;21,38,22;,
  3;22,38,39;,
  3;22,39,23;,
  3;23,39,40;,
  3;23,40,24;,
  3;24,40,41;,
  3;24,41,25;,
  3;25,41,42;,
  3;25,42,26;,
  3;26,42,43;,
  3;26,43,27;,
  3;27,43,44;,
  3;27,44,28;,
  3;28,44,45;,
  3;28,45,29;,
  3;29,45,46;,
  3;29,46,30;,
  3;30,46,47;,
  3;30,47,31;,
  3;31,47,48;,
  3;31,48,32;,
  3;32,48,163;,
  3;32,163,162;,
  3;33,49,50;,
  3;33,50,34;,
  3;34,50,51;,
  3;34,51,35;,
  3;35,51,52;,
  3;35,52,36;,
  3;36,52,53;,
  3;36,53,37;,
  3;37,53,54;,
  3;37,54,38;,
  3;38,54,55;,
  3;38,55,39;,
  3;39,55,56;,
  3;39,56,40;,
  3;40,56,57;,
  3;40,57,41;,
  3;41,57,58;,
  3;41,58,42;,
  3;42,58,59;,
  3;42,59,43;,
  3;43,59,60;,
  3;43,60,44;,
  3;44,60,61;,
  3;44,61,45;,
  3;45,61,62;,
  3;45,62,46;,
  3;46,62,63;,
  3;46,63,47;,
  3;47,63,64;,
  3;47,64,48;,
  3;48,64,164;,
  3;48,164,163;,
  3;49,65,66;,
  3;49,66,50;,
  3;50,66,67;,
  3;50,67,51;,
  3;51,67,68;,
  3;51,68,52;,
  3;52,68,69;,
  3;52,69,53;,
  3;53,69,70;,
  3;53,70,54;,
  3;54,70,71;,
  3;54,71,55;,
  3;55,71,72;,
  3;55,72,56;,
  3;56,72,73;,
  3;56,73,57;,
  3;57,73,74;,
  3;57,74,58;,
  3;58,74,75;,
  3;58,75,59;,
  3;59,75,76;,
  3;59,76,60;,
  3;60,76,77;,
  3;60,77,61;,
  3;61,77,78;,
  3;61,78,62;,
  3;62,78,79;,
  3;62,79,63;,
  3;63,79,80;,
  3;63,80,64;,
  3;64,80,165;,
  3;64,165,164;,
  3;65,81,82;,
  3;65,82,66;,
  3;66,82,83;,
  3;66,83,67;,
  3;67,83,84;,
  3;67,84,68;,
  3;68,84,85;,
  3;68,85,69;,
  3;69,85,86;,
  3;69,86,70;,
  3;70,86,87;,
  3;70,87,71;,
  3;71,87,88;,
  3;71,88,72;,
  3;72,88,89;,
  3;72,89,73;,
  3;73,89,90;,
  3;73,90,74;,
  3;74,90,91;,
  3;74,91,75;,
  3;75,91,92;,
  3;75,92,76;,
  3;76,92,93;,
  3;76,93,77;,
  3;77,93,94;,
  3;77,94,78;,
  3;78,94,95;,
  3;78,95,79;,
  3;79,95,96;,
  3;79,96,80;,
  3;80,96,166;,
  3;80,166,165;,
  3;81,97,98;,
  3;81,98,82;,
  3;82,98,99;,
  3;82,99,83;,
  3;83,99,100;,
  3;83,100,84;,
  3;84,100,101;,
  3;84,101,85;,
  3;85,101,102;,
  3;85,102,86;,
  3;86,102,103;,
  3;86,103,87;,
  3;87,103,104;,
  3;87,104,88;,
  3;88,104,105;,
  3;88,105,89;,
  3;89,105,106;,
  3;89,106,90;,
  3;90,106,107;,
  3;90,107,91;,
  3;91,107,108;,
  3;91,108,92;,
  3;92,108,109;,
  3;92,109,93;,
  3;93,109,110;,
  3;93,110,94;,
  3;94,110,111;,
  3;94,111,95;,
  3;95,111,112;,
  3;95,112,96;,
  3;96,112,167;,
  3;96,167,166;,
  3;113,98,97;,
  3;168,99,98;,
  3;169,100,99;,
  3;170,101,100;,
  3;171,102,101;,
  3;172,103,102;,
  3;173,104,103;,
  3;174,105,104;,
  3;175,106,105;,
  3;176,107,106;,
  3;177,108,107;,
  3;178,109,108;,
  3;179,110,109;,
  3;180,111,110;,
  3;181,112,111;,
  3;182,167,112;,
  3;117,114,116;,
  3;114,117,115;,
  3;118,183,121;,
  3;183,118,184;,
  3;185,119,114;,
  3;119,185,186;,
  3;187,188,117;,
  3;188,187,120;,
  3;189,190,191;,
  3;190,189,192;,
  3;125,124,122;,
  3;122,123,125;,
  3;126,129,193;,
  3;193,194,126;,
  3;195,122,127;,
  3;127,196,195;,
  3;197,125,198;,
  3;198,128,197;,
  3;199,200,201;,
  3;201,202,199;,
  3;131,135,134;,
  3;135,131,130;,
  3;136,133,137;,
  3;133,136,132;,
  3;203,204,205;,
  3;204,203,206;,
  3;207,208,133;,
  3;208,207,130;,
  3;209,210,211;,
  3;210,209,212;,
  3;139,142,143;,
  3;143,138,139;,
  3;144,145,141;,
  3;141,140,144;,
  3;213,214,215;,
  3;215,216,213;,
  3;217,141,218;,
  3;218,138,217;,
  3;219,220,221;,
  3;221,222,219;;

  MeshNormals  {
   127;
   -0.000000;0.000000;1.000000;,
   -0.022464;0.418540;0.907920;,
   -0.180922;0.378084;0.907920;,
   -0.311837;0.280068;0.907921;,
   -0.395277;0.139414;0.907920;,
   -0.418540;-0.022464;0.907920;,
   -0.378084;-0.180922;0.907920;,
   -0.280068;-0.311837;0.907920;,
   -0.139414;-0.395277;0.907920;,
   0.022464;-0.418540;0.907920;,
   0.180922;-0.378084;0.907920;,
   0.311837;-0.280068;0.907920;,
   0.395277;-0.139414;0.907920;,
   0.418540;0.022464;0.907920;,
   0.378084;0.180923;0.907920;,
   0.280068;0.311837;0.907920;,
   0.139414;0.395277;0.907920;,
   -0.009152;0.706555;0.707599;,
   -0.278842;0.649269;0.707599;,
   -0.506081;0.493139;0.707599;,
   -0.656274;0.261932;0.707599;,
   -0.706555;-0.009151;0.707599;,
   -0.649270;-0.278841;0.707599;,
   -0.493139;-0.506081;0.707599;,
   -0.261932;-0.656274;0.707599;,
   0.009151;-0.706555;0.707599;,
   0.278842;-0.649269;0.707599;,
   0.506081;-0.493139;0.707599;,
   0.656274;-0.261932;0.707599;,
   0.706555;0.009152;0.707599;,
   0.649269;0.278842;0.707599;,
   0.493138;0.506081;0.707599;,
   0.261932;0.656274;0.707599;,
   -0.004889;0.923671;0.383155;,
   -0.357990;0.851490;0.383155;,
   -0.656591;0.649678;0.383155;,
   -0.855231;0.348957;0.383155;,
   -0.923671;-0.004888;0.383155;,
   -0.851490;-0.357990;0.383155;,
   -0.649678;-0.656590;0.383155;,
   -0.348958;-0.855231;0.383155;,
   0.004888;-0.923671;0.383155;,
   0.357990;-0.851490;0.383155;,
   0.656591;-0.649677;0.383155;,
   0.855232;-0.348957;0.383155;,
   0.923671;0.004889;0.383155;,
   0.851490;0.357990;0.383155;,
   0.649677;0.656591;0.383155;,
   0.348957;0.855232;0.383155;,
   -0.000000;1.000000;0.000000;,
   -0.382683;0.923880;-0.000000;,
   -0.707107;0.707107;-0.000000;,
   -0.923880;0.382683;-0.000000;,
   -1.000000;0.000000;-0.000000;,
   -0.923880;-0.382683;-0.000000;,
   -0.707107;-0.707107;-0.000000;,
   -0.382684;-0.923880;-0.000000;,
   0.000000;-1.000000;0.000000;,
   0.382684;-0.923880;-0.000000;,
   0.707107;-0.707107;-0.000000;,
   0.923880;-0.382683;-0.000000;,
   1.000000;0.000001;-0.000000;,
   0.923879;0.382684;-0.000000;,
   0.707106;0.707107;-0.000000;,
   0.382683;0.923880;-0.000000;,
   0.004888;0.923671;-0.383155;,
   -0.348957;0.855231;-0.383155;,
   -0.649678;0.656591;-0.383155;,
   -0.851490;0.357990;-0.383155;,
   -0.923671;0.004888;-0.383155;,
   -0.855232;-0.348957;-0.383155;,
   -0.656591;-0.649677;-0.383155;,
   -0.357990;-0.851490;-0.383155;,
   -0.004888;-0.923671;-0.383155;,
   0.348958;-0.855231;-0.383155;,
   0.649678;-0.656590;-0.383155;,
   0.851490;-0.357989;-0.383155;,
   0.923671;-0.004888;-0.383155;,
   0.855231;0.348958;-0.383155;,
   0.656590;0.649678;-0.383155;,
   0.357989;0.851490;-0.383155;,
   0.009151;0.706555;-0.707599;,
   -0.261932;0.656274;-0.707599;,
   -0.493139;0.506081;-0.707599;,
   -0.649270;0.278842;-0.707599;,
   -0.706555;0.009152;-0.707599;,
   -0.656274;-0.261932;-0.707599;,
   -0.506081;-0.493139;-0.707599;,
   -0.278842;-0.649270;-0.707599;,
   -0.009151;-0.706555;-0.707599;,
   0.261932;-0.656274;-0.707599;,
   0.493139;-0.506081;-0.707599;,
   0.649270;-0.278841;-0.707599;,
   0.706555;-0.009151;-0.707599;,
   0.656273;0.261933;-0.707599;,
   0.506080;0.493139;-0.707599;,
   0.278841;0.649270;-0.707599;,
   0.022464;0.418540;-0.907920;,
   -0.139414;0.395277;-0.907920;,
   -0.280068;0.311837;-0.907920;,
   -0.378084;0.180922;-0.907920;,
   -0.418540;0.022464;-0.907920;,
   -0.395277;-0.139414;-0.907920;,
   -0.311837;-0.280068;-0.907920;,
   -0.180922;-0.378084;-0.907920;,
   -0.022464;-0.418540;-0.907920;,
   0.139414;-0.395277;-0.907920;,
   0.280068;-0.311837;-0.907920;,
   0.378084;-0.180922;-0.907920;,
   0.418540;-0.022464;-0.907920;,
   0.395277;0.139415;-0.907920;,
   0.311837;0.280068;-0.907920;,
   0.180922;0.378084;-0.907920;,
   -0.000000;0.000000;-1.000000;,
   1.000000;0.000000;0.000000;,
   0.000000;0.000000;-1.000000;,
   0.000000;0.000000;-1.000000;,
   0.000000;1.000000;0.000000;,
   0.000000;-1.000000;0.000000;,
   0.000000;0.000000;1.000000;,
   0.000000;1.000000;0.000000;,
   -1.000000;-0.000000;0.000000;,
   0.000000;0.000000;-1.000000;,
   1.000000;0.000000;0.000000;,
   -1.000000;0.000000;0.000000;,
   1.000000;0.000000;0.000000;,
   -1.000000;-0.000000;0.000000;;
   264;
   3;0,1,2;,
   3;0,2,3;,
   3;0,3,4;,
   3;0,4,5;,
   3;0,5,6;,
   3;0,6,7;,
   3;0,7,8;,
   3;0,8,9;,
   3;0,9,10;,
   3;0,10,11;,
   3;0,11,12;,
   3;0,12,13;,
   3;0,13,14;,
   3;0,14,15;,
   3;0,15,16;,
   3;0,16,1;,
   3;1,17,18;,
   3;1,18,2;,
   3;2,18,19;,
   3;2,19,3;,
   3;3,19,20;,
   3;3,20,4;,
   3;4,20,21;,
   3;4,21,5;,
   3;5,21,22;,
   3;5,22,6;,
   3;6,22,23;,
   3;6,23,7;,
   3;7,23,24;,
   3;7,24,8;,
   3;8,24,25;,
   3;8,25,9;,
   3;9,25,26;,
   3;9,26,10;,
   3;10,26,27;,
   3;10,27,11;,
   3;11,27,28;,
   3;11,28,12;,
   3;12,28,29;,
   3;12,29,13;,
   3;13,29,30;,
   3;13,30,14;,
   3;14,30,31;,
   3;14,31,15;,
   3;15,31,32;,
   3;15,32,16;,
   3;16,32,17;,
   3;16,17,1;,
   3;17,33,34;,
   3;17,34,18;,
   3;18,34,35;,
   3;18,35,19;,
   3;19,35,36;,
   3;19,36,20;,
   3;20,36,37;,
   3;20,37,21;,
   3;21,37,38;,
   3;21,38,22;,
   3;22,38,39;,
   3;22,39,23;,
   3;23,39,40;,
   3;23,40,24;,
   3;24,40,41;,
   3;24,41,25;,
   3;25,41,42;,
   3;25,42,26;,
   3;26,42,43;,
   3;26,43,27;,
   3;27,43,44;,
   3;27,44,28;,
   3;28,44,45;,
   3;28,45,29;,
   3;29,45,46;,
   3;29,46,30;,
   3;30,46,47;,
   3;30,47,31;,
   3;31,47,48;,
   3;31,48,32;,
   3;32,48,33;,
   3;32,33,17;,
   3;33,49,50;,
   3;33,50,34;,
   3;34,50,51;,
   3;34,51,35;,
   3;35,51,52;,
   3;35,52,36;,
   3;36,52,53;,
   3;36,53,37;,
   3;37,53,54;,
   3;37,54,38;,
   3;38,54,55;,
   3;38,55,39;,
   3;39,55,56;,
   3;39,56,40;,
   3;40,56,57;,
   3;40,57,41;,
   3;41,57,58;,
   3;41,58,42;,
   3;42,58,59;,
   3;42,59,43;,
   3;43,59,60;,
   3;43,60,44;,
   3;44,60,61;,
   3;44,61,45;,
   3;45,61,62;,
   3;45,62,46;,
   3;46,62,63;,
   3;46,63,47;,
   3;47,63,64;,
   3;47,64,48;,
   3;48,64,49;,
   3;48,49,33;,
   3;49,65,66;,
   3;49,66,50;,
   3;50,66,67;,
   3;50,67,51;,
   3;51,67,68;,
   3;51,68,52;,
   3;52,68,69;,
   3;52,69,53;,
   3;53,69,70;,
   3;53,70,54;,
   3;54,70,71;,
   3;54,71,55;,
   3;55,71,72;,
   3;55,72,56;,
   3;56,72,73;,
   3;56,73,57;,
   3;57,73,74;,
   3;57,74,58;,
   3;58,74,75;,
   3;58,75,59;,
   3;59,75,76;,
   3;59,76,60;,
   3;60,76,77;,
   3;60,77,61;,
   3;61,77,78;,
   3;61,78,62;,
   3;62,78,79;,
   3;62,79,63;,
   3;63,79,80;,
   3;63,80,64;,
   3;64,80,65;,
   3;64,65,49;,
   3;65,81,82;,
   3;65,82,66;,
   3;66,82,83;,
   3;66,83,67;,
   3;67,83,84;,
   3;67,84,68;,
   3;68,84,85;,
   3;68,85,69;,
   3;69,85,86;,
   3;69,86,70;,
   3;70,86,87;,
   3;70,87,71;,
   3;71,87,88;,
   3;71,88,72;,
   3;72,88,89;,
   3;72,89,73;,
   3;73,89,90;,
   3;73,90,74;,
   3;74,90,91;,
   3;74,91,75;,
   3;75,91,92;,
   3;75,92,76;,
   3;76,92,93;,
   3;76,93,77;,
   3;77,93,94;,
   3;77,94,78;,
   3;78,94,95;,
   3;78,95,79;,
   3;79,95,96;,
   3;79,96,80;,
   3;80,96,81;,
   3;80,81,65;,
   3;81,97,98;,
   3;81,98,82;,
   3;82,98,99;,
   3;82,99,83;,
   3;83,99,100;,
   3;83,100,84;,
   3;84,100,101;,
   3;84,101,85;,
   3;85,101,102;,
   3;85,102,86;,
   3;86,102,103;,
   3;86,103,87;,
   3;87,103,104;,
   3;87,104,88;,
   3;88,104,105;,
   3;88,105,89;,
   3;89,105,106;,
   3;89,106,90;,
   3;90,106,107;,
   3;90,107,91;,
   3;91,107,108;,
   3;91,108,92;,
   3;92,108,109;,
   3;92,109,93;,
   3;93,109,110;,
   3;93,110,94;,
   3;94,110,111;,
   3;94,111,95;,
   3;95,111,112;,
   3;95,112,96;,
   3;96,112,97;,
   3;96,97,81;,
   3;113,98,97;,
   3;113,99,98;,
   3;113,100,99;,
   3;113,101,100;,
   3;113,102,101;,
   3;113,103,102;,
   3;113,104,103;,
   3;113,105,104;,
   3;113,106,105;,
   3;113,107,106;,
   3;113,108,107;,
   3;113,109,108;,
   3;113,110,109;,
   3;113,111,110;,
   3;113,112,111;,
   3;113,97,112;,
   3;114,114,114;,
   3;114,114,114;,
   3;115,115,116;,
   3;115,115,115;,
   3;117,117,117;,
   3;117,117,117;,
   3;118,118,118;,
   3;118,118,118;,
   3;119,119,119;,
   3;119,119,119;,
   3;120,120,120;,
   3;120,120,120;,
   3;121,121,121;,
   3;121,121,121;,
   3;122,122,122;,
   3;122,122,122;,
   3;119,119,119;,
   3;119,119,119;,
   3;123,123,123;,
   3;123,123,123;,
   3;117,117,117;,
   3;117,117,117;,
   3;118,118,118;,
   3;118,118,118;,
   3;119,119,119;,
   3;119,119,119;,
   3;124,124,124;,
   3;124,124,124;,
   3;115,115,115;,
   3;115,115,115;,
   3;122,122,122;,
   3;122,122,122;,
   3;119,119,119;,
   3;119,119,119;,
   3;125,125,125;,
   3;125,125,125;,
   3;118,118,118;,
   3;118,118,118;,
   3;126,126,126;,
   3;126,126,126;;
  }

  MeshMaterialList  {
   1;
   264;
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
   0,
   0,
   0,
   0,
   0,
   0,
   0,
   0,
   0;

   Material {
    0.600000;0.894118;0.600000;1.000000;;
    0.000000;
    0.600000;0.894118;0.600000;;
    0.000000;0.000000;0.000000;;
   }
  }

  MeshTextureCoords  {
   223;
   0.000000;0.000000;,
   0.000000;0.125000;,
   0.062500;0.125000;,
   0.125000;0.125000;,
   0.187500;0.125000;,
   0.250000;0.125000;,
   0.312500;0.125000;,
   0.375000;0.125000;,
   0.437500;0.125000;,
   0.500000;0.125000;,
   0.562500;0.125000;,
   0.625000;0.125000;,
   0.687500;0.125000;,
   0.750000;0.125000;,
   0.812500;0.125000;,
   0.875000;0.125000;,
   0.937500;0.125000;,
   0.000000;0.250000;,
   0.062500;0.250000;,
   0.125000;0.250000;,
   0.187500;0.250000;,
   0.250000;0.250000;,
   0.312500;0.250000;,
   0.375000;0.250000;,
   0.437500;0.250000;,
   0.500000;0.250000;,
   0.562500;0.250000;,
   0.625000;0.250000;,
   0.687500;0.250000;,
   0.750000;0.250000;,
   0.812500;0.250000;,
   0.875000;0.250000;,
   0.937500;0.250000;,
   0.000000;0.375000;,
   0.062500;0.375000;,
   0.125000;0.375000;,
   0.187500;0.375000;,
   0.250000;0.375000;,
   0.312500;0.375000;,
   0.375000;0.375000;,
   0.437500;0.375000;,
   0.500000;0.375000;,
   0.562500;0.375000;,
   0.625000;0.375000;,
   0.687500;0.375000;,
   0.750000;0.375000;,
   0.812500;0.375000;,
   0.875000;0.375000;,
   0.937500;0.375000;,
   0.000000;0.500000;,
   0.062500;0.500000;,
   0.125000;0.500000;,
   0.187500;0.500000;,
   0.250000;0.500000;,
   0.312500;0.500000;,
   0.375000;0.500000;,
   0.437500;0.500000;,
   0.500000;0.500000;,
   0.562500;0.500000;,
   0.625000;0.500000;,
   0.687500;0.500000;,
   0.750000;0.500000;,
   0.812500;0.500000;,
   0.875000;0.500000;,
   0.937500;0.500000;,
   0.000000;0.625000;,
   0.062500;0.625000;,
   0.125000;0.625000;,
   0.187500;0.625000;,
   0.250000;0.625000;,
   0.312500;0.625000;,
   0.375000;0.625000;,
   0.437500;0.625000;,
   0.500000;0.625000;,
   0.562500;0.625000;,
   0.625000;0.625000;,
   0.687500;0.625000;,
   0.750000;0.625000;,
   0.812500;0.625000;,
   0.875000;0.625000;,
   0.937500;0.625000;,
   0.000000;0.750000;,
   0.062500;0.750000;,
   0.125000;0.750000;,
   0.187500;0.750000;,
   0.250000;0.750000;,
   0.312500;0.750000;,
   0.375000;0.750000;,
   0.437500;0.750000;,
   0.500000;0.750000;,
   0.562500;0.750000;,
   0.625000;0.750000;,
   0.687500;0.750000;,
   0.750000;0.750000;,
   0.812500;0.750000;,
   0.875000;0.750000;,
   0.937500;0.750000;,
   0.000000;0.875000;,
   0.062500;0.875000;,
   0.125000;0.875000;,
   0.187500;0.875000;,
   0.250000;0.875000;,
   0.312500;0.875000;,
   0.375000;0.875000;,
   0.437500;0.875000;,
   0.500000;0.875000;,
   0.562500;0.875000;,
   0.625000;0.875000;,
   0.687500;0.875000;,
   0.750000;0.875000;,
   0.812500;0.875000;,
   0.875000;0.875000;,
   0.937500;0.875000;,
   0.000000;1.000000;,
   0.000000;1.000000;,
   1.000000;1.000000;,
   0.000000;0.000000;,
   1.000000;0.000000;,
   0.550628;1.000000;,
   0.550628;1.000000;,
   0.449372;1.000000;,
   0.550628;0.000000;,
   0.000000;1.000000;,
   1.000000;1.000000;,
   0.000000;0.000000;,
   1.000000;0.000000;,
   0.550173;1.000000;,
   0.550173;1.000000;,
   0.449827;1.000000;,
   0.550173;0.000000;,
   1.000000;1.000000;,
   1.000000;0.000000;,
   0.000000;1.000000;,
   0.000000;0.000000;,
   0.446232;0.000000;,
   0.446232;1.000000;,
   0.553768;1.000000;,
   0.553768;0.000000;,
   1.000000;1.000000;,
   1.000000;0.000000;,
   0.000000;1.000000;,
   0.000000;0.000000;,
   0.450670;0.000000;,
   0.450670;1.000000;,
   0.549330;1.000000;,
   0.549330;0.000000;,
   0.062500;0.000000;,
   0.125000;0.000000;,
   0.187500;0.000000;,
   0.250000;0.000000;,
   0.312500;0.000000;,
   0.375000;0.000000;,
   0.437500;0.000000;,
   0.500000;0.000000;,
   0.562500;0.000000;,
   0.625000;0.000000;,
   0.687500;0.000000;,
   0.750000;0.000000;,
   0.812500;0.000000;,
   0.875000;0.000000;,
   0.937500;0.000000;,
   1.000000;0.125000;,
   1.000000;0.250000;,
   1.000000;0.375000;,
   1.000000;0.500000;,
   1.000000;0.625000;,
   1.000000;0.750000;,
   1.000000;0.875000;,
   0.062500;1.000000;,
   0.125000;1.000000;,
   0.187500;1.000000;,
   0.250000;1.000000;,
   0.312500;1.000000;,
   0.375000;1.000000;,
   0.437500;1.000000;,
   0.500000;1.000000;,
   0.562500;1.000000;,
   0.625000;1.000000;,
   0.687500;1.000000;,
   0.750000;1.000000;,
   0.812500;1.000000;,
   0.875000;1.000000;,
   0.937500;1.000000;,
   0.000000;0.000000;,
   0.000000;1.000000;,
   0.000000;0.000000;,
   0.550628;0.000000;,
   1.000000;1.000000;,
   0.449372;0.000000;,
   1.000000;1.000000;,
   0.449372;0.000000;,
   1.000000;0.000000;,
   0.449372;1.000000;,
   0.000000;0.000000;,
   0.000000;1.000000;,
   0.000000;0.000000;,
   0.550173;0.000000;,
   1.000000;1.000000;,
   0.449827;0.000000;,
   1.000000;1.000000;,
   1.000000;0.000000;,
   0.449827;0.000000;,
   0.449827;1.000000;,
   0.553768;1.000000;,
   0.000000;0.000000;,
   0.553768;0.000000;,
   0.000000;1.000000;,
   1.000000;0.000000;,
   0.000000;1.000000;,
   1.000000;1.000000;,
   0.446232;0.000000;,
   1.000000;0.000000;,
   0.446232;1.000000;,
   0.549330;1.000000;,
   0.549330;0.000000;,
   0.000000;0.000000;,
   0.000000;1.000000;,
   1.000000;0.000000;,
   0.000000;1.000000;,
   1.000000;1.000000;,
   1.000000;0.000000;,
   0.450670;0.000000;,
   0.450670;1.000000;;
  }
 }
}