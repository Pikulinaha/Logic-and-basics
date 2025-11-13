#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <conio.h>
#include <stdlib.h>
#include <time.h>
#include <queue>

using namespace std;



void BFS(int** G, int numG, int* visited, int s) {
    queue<int>q;
    int v;

    visited[s] = 1;
    q.push(s);

    while (!q.empty()) {

        v = q.front();
        q.pop();
        printf("%3d", v);

        for (int i = 0; i < numG; i++) {
            if (G[v][i] == 1 && visited[i] == 0) {
                q.push(i);
                visited[i] = 1;
            }
        }
    }
}

int main() {

    int** G;
    int numG, current;
    int* visited;

    printf("Input number of vertission:");
    scanf("%d", &numG);

    visited = (int*)malloc(numG * sizeof(int));
    G = (int**)malloc(numG * sizeof(int*));
    for (int i = 0; i < numG; i++)
        G[i] = (int*)malloc(numG * sizeof(int));

    srand(time(NULL));

    for (int i = 0; i < numG; i++) {
        visited[i] = 0;
        for (int j = 0; j < numG; j++) {
            G[i][j] = G[i][j] = (i == j ? 0 : rand() % 2);
        }
    }

    for (int i = 0; i < numG; i++) {
        for (int j = 0; j < numG; j++) {
            printf("%3d", G[i][j]);
        }
        printf("\n");
    }


    printf("Input the start vert:");
    scanf("%d", &current);

    printf("\nPath: ");

    BFS(G, numG, visited, current);
    printf("\n\n");

    free(visited);
    for (int i = 0; i < numG; i++)
        free(G[i]);
    free(G);


    return 0;
}