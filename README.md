# BIA601_F23_HW
This repository contains the assignment submission for BIA601 F23 (Basic Intelligent Algorithms 601 Fall 2023). It includes both the backend and frontend code necessary to run the application, as well as the assignment PDF for reference.

## Table of Contents
- [Overview](#overview)
- [Modes](#modes)
  - [Mode 1](#mode-1)
  - [Mode 2](#mode-2)
  - [Mode 3](#mode-3)
  - [Mode 4](#mode-4)
  - [Mode 5](#mode-5)
  - [Mode 6](#mode-6)

## Overview

This project addresses several optimization challenges, including various scenarios involving the knapsack problem and the traveling salesman problem (TSP). The backend is developed using ASP.NET, while the frontend is built with React.

## Modes

### Mode 1

In this mode, the application identifies the optimal set of items that fit into a knapsack and determines the best route between multiple cities. This involves solving the 0/1 Knapsack problem and the TSP independently.

### Mode 2

Mode 2 builds on Mode 1 by associating each item with a specific city. The objective is to select items that fit into the knapsack and deliver them to their respective locations, without the necessity of visiting all cities.

### Mode 3

This mode introduces multiple salesmen, each with their own knapsack, distributing the items among them.

### Mode 4

Here, warehouses are introduced, with each vehicle having a distinct starting and ending point. The solution involves using dummy nodes to represent these points, ensuring that each route begins and ends appropriately.

### Mode 5

In this mode, items are linked to specific warehouses, meaning each item has a designated pick-up and drop-off location.

### Mode 6

This mode incorporates a time penalty for items, which is proportional to their weight and value. This represents the real-world scenario where picking up and dropping off items requires time.
