import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CreateTaskRequest } from '../models/create-task-request';

@Injectable({
  providedIn: 'root'
})
export class TaskService {

  constructor() { }

  #httpClient = inject(HttpClient);


  createTask(description: string, title: string, projectId: number, storyId: number) {
    return this.#httpClient.post<CreateTaskRequest>("/api/task", {
      Description: description,
      Subject: title,
      ProjectId: projectId,
      UserStoryId: storyId,
    });
  }
}
