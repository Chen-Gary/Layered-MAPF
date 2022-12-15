# A Layered Approach for Multi-Agent Path Finding

Simulation platform of a Layered Approach for Multi-Agent Path Finding (MAPF).

## 1. Introduction

*Layered MAPF* is a newly proposed algorithm for solving Multi-Agent Path Finding (MAPF) problems. This algorithm adopts a **hybrid cloud-edge-terminal architecture**, which consists of one centralized cloud server and multiple decentralized edge servers and terminals (robots). The robots plan their path using a two-level A* algorithm guided by **heat maps**, improving computing efficiency and avoiding most potential conflicts.



## 2. Demo

* **2D Simulation View**

  ![](README_img/demo1/2d_view.png)

* **3D Simulation View**

  ![](README_img/demo1/3d_view.png)

* **Heat Map Visualizer**

  * Heat Map using T-Shape Generation Algorithm

    ![](README_img/demo1/heatmap_visualizer_TShape.png)

  * Heat Map using Circle-Gaussian Generation Algorithm

    ![](README_img/demo1/heatmap_visualizer_CircleGaussian.png)

* **Map Editor**

  Edit grid map with MS Excel and save as `csv` files.

  ![](README_img/demo1/map_editor.png)

* **Map Converter**

  Convert `csv` maps (including robot arrangements) to `json` files. The simulation program will read `json` files at runtime before the simulation begins.

  ![](README_img/demo1/map_converter.png)

* **Random Task Generator**

  Generate random tasks given the `json` maps and corresponding robot arrangements.

  ![](README_img/demo1/task_generator.png)



## 3. Algorithm Architecture

**Layered Architecture:**

![](README_img/report/layer_struct.png)



**Flow Chart:**

![](README_img/report/layer_flow.png)



### 3.1 Offline Preprocessing Phase

Original Map:

![](README_img/report/phase1/phase1_1.png)

Partitioned Map and Topological Map: 

![](README_img/report/phase1/phase1_2.png)



### 3.2 Online Scheduling Phase

Online Scheduling Phase: Two-level A* algorithm guided by **heat maps**

* Heat Map Generation (For one single agent):

  * T-Shape Generation Algorithm is shown as an example.

  ![](README_img/report/heatmap_1.png)

* Complete Heat Map (with all agents included):

  ![](README_img/report/heatmap_2.png)



## 4. Environment

* Unity 2019.4.5f1