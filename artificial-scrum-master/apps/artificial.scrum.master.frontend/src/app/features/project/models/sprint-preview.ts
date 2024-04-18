export interface SprintPreview {
  projectId: number;
  projectName: string;
  projectSlug: string;
  sprintId: number;
  sprintName: string;
  estimatedStart: string;
  estimatedFinish: string;
  userStories: UserStoryPrweview[];
}

export interface UserStoryPrweview {
  userStoryId: number;
  userStoryName: string;
  statusName: string;
  isClosed: boolean;
  userStoryRef: number;
  totalPoints: number;
}
