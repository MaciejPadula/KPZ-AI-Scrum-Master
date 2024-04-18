import { TaskEstimation } from "./task-estimation";

export interface GetTaskEstimationsResponse {
  estimations: TaskEstimation[];
  averageEstimation: number;
}