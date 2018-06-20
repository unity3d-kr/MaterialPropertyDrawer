## [MaterialToggleLeft]
Unity [MaterialToggle] attribute has checkbox on right. <br />
New [MaterialToggleLeft] attribute seems like GUI [ToggleLeft].<br />
<br />
[MaterialToggle] => label □<br />
[MaterialToggleLeft] => □ label<br />
<br />
It can use also [ToggleLeft]<br />
<br />
Reference<br />
https://github.com/MattRix/UnityDecompiled/blob/5.6.0f3/UnityEditor/UnityEditor/MaterialToggleDrawer.cs
<br />

## [BlendMode]
Unity Standard Shader use 'Rendering Mode' popup.<br />
But, it built in StandardShaderGUI.cs<br />
// Blending state<br />
[HideInInspector] _Mode ("__mode", Float) = 0.0<br />
[HideInInspector] _SrcBlend ("__src", Float) = 1.0<br />
[HideInInspector] _DstBlend ("__dst", Float) = 0.0<br />
[HideInInspector] _ZWrite ("__zw", Float) = 1.0<br />
<br />
It change to<br />
[BlendMode] _Mode("Rendering Mode", Float) = 0.0<br />
or,<br />
[BlendMode(_SrcBlend, _DstBlend, _ZWrite)] _Mode("Rendering Mode", Float) = 0.0<br />
<br />

## [Normalize]
Vector normalize in shader inspector.<br />
[Normalize] _Direction("Normalized Direction",Vector) = (0,1,0,0) // Vector3 Normalize<br />
[Normalize(3)] _Direction("Normalized Direction",Vector) = (0,1,0,0) // Vector3 Normalize<br />
[Normalize(2)] _Direction("Normalized Direction",Vector) = (0,1,0,0) // Vector2 Normalize<br />
[Normalize(4)] _Direction("Normalized Direction",Vector) = (0,1,0,0) // Vector4 Normalize<br />
<br />

## [VectorField(x,y,z,w)]
It label to vector member.<br />
[VectorField(param1, param2, param3, param4] _Param("Params",Vector) = (1,2,3,4)<br />
<br />
Params x[ 1 ] y[ 2 ] z[ 3 ] w[ 4 ]<br />
change to:<br />
Params<br />
&nbsp;&nbsp;&nbsp;param1 [ 1 ]<br />
&nbsp;&nbsp;&nbsp;param2 [ 2 ]<br />
&nbsp;&nbsp;&nbsp;param3 [ 3 ]<br />
&nbsp;&nbsp;&nbsp;param4 [ 4 ]<br />
<br />

## [TextureToggle]
It support materialToggle when texture exist.<br />
[TextureToggle(KEYWORD)] _MainTex("Main", 2D) = "white" {}<br />
if you set _MainTex in inspector, material set EnableKeyword("KEYWORD_ON").<br />
<br />

## [AlphaRange]
It show Color(RGB) picker and Slider of Alpha Value.<br />
Usage:<br />
 [AlphaRange] _Color("Color", Color) = (1, 1, 1, 0.5)<br />
 [AlphaRange(Name)] ...<br />
 [AlphaRange(Name, maxValue)] ...<br />
 [AlphaRange(Name, minValue, maxValue)] ...<br />