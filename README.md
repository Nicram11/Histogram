# Histogram

## Introduction
The task of this program is to display produkt grades as a histogram.

## Technologies
Project is created for platform .NET 6.0
with help of graphics engine WPF 

## Features
Product Grades are computed from text files (one .txt for one product) formatted in such a way, where each line contains an grade ended with semicolon, e.g.\
1;\
5;\
3;\
1;\
then results of these calculations are displayed on the graph

Program supports comparing multiple product grades in one graph. To make it easier options, for hiding/showing a chosen product and changing the colour assigned to them, have been added.

The results of computing grades can be saved to a text file.

While shutting down the program saves paths to added products grades so when running the program again the state of the application before it was closed is restored.

## Appciation Interface
![image](https://user-images.githubusercontent.com/108276673/176774973-4927c069-7343-4acb-a4ee-05c08b6f6f90.png)

## Status
I'm planning to extend the program with the possibility of flexible choise of grades range with has to be displayed. That is why, for now there is inefficient saving data in BST structure. In future in this stucture will be saved unconventional grades range.
