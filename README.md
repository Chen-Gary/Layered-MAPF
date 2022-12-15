# A Layered Approach for Multi-Agent Path Finding

Simulation platform of a Layered Approach for Multi-Agent Path Finding (MAPF).

## 1. Introduction

*Layered MAPF* is a newly proposed algorithm for solving Multi-Agent Path Finding (MAPF) problems. This algorithm adopts a **hybrid cloud-edge-terminal architecture**, which consists of one centralized cloud server and multiple decentralized edge servers and terminals (robots). The robots plan their path using a two-level A* algorithm guided by **heat maps**, improving computing efficiency and avoiding most potential conflicts.



## 2. Demo



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