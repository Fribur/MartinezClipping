# Martinez-Rueda polygon clipping algorithm



---------------

This is a C# conversion of https://github.com/w8r/martinez. 

Detailed description of the algorithm is in the paper 
[A new algorithm for computing Boolean operations on polygons (2008, 2013) by Francisco Martinez, Antonio Jesus Rueda, Francisco Ramon Feito (and its C++ code)](https://www.sciencedirect.com/science/article/abs/pii/S0965997813000379).

## 2) Compiling
------------

Open MartinezClipping.sln in Visual Studio.

## 3) Executing
------------

To compute the intersection (or union) of two polygon, execute

- `MartinezClipping.exe [-union] input1.poly input2.poly output.poly`

where "-union" is the optional switch for computing the union
instead of the intersection, "input?.poly" are the names of the
two input polygon files and "output.poly" is the name of the
result polygon file.


## 4) File Format
--------------

The "*.poly" files must have the following structure. Each line
contains two numbers (int or double), the x and the y coordinates
of a vertex, followed by a "," or a ";", where the "," is used to
separate the vertices of a polygon component and ";" marks the end
of the component. For example, the following 7 lines:

~~~~
0 0,  
1 0,  
0 1;  
-0.5 -0.5,  
1.5 -0.5,  
1.5 1.5,  
-0.5 1.5;
~~~~

describe a polygon with 2 components, a right triangle inside a
square. All vertices in one file must be different from each
other.


## 5) Admitted Input
-----------------

The following features are allowed in the input polygons:

- the vertex order in each component can be CW or CCW
- components can be nested (AKA holes)
- the two input polygons are allowed to have degenerate
  intersections (vertex on edge, overlapping edges, etc.)
  with each other

The following features are not allowed in the input polygons:

- the polygons should not self-intersect (including degenerate
  self-intersections like vertex on vertex, vertex on edge),
  although the result will be correct as long as the self-
  intersection does not lie on the other polygon


## 6) Robustness
-------------

The implementation is based on C# floating point numbers with
double precision and therefore not robust. The EPSILON parameter
(set to 0.000000001) is used as a tolerance for most equality checks,
and two numbers are considered equal if their difference is less
than EPSILON.
