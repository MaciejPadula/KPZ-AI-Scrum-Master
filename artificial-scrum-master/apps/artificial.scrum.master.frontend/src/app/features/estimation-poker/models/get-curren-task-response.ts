import { SessionTask } from "./session-task";

export interface GetCurrentTaskResponse {
  currentTask: SessionTask | null;
}