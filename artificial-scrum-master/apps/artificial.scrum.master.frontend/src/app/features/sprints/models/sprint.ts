export interface Sprint {
  projectId: number;
  projectName: string;
  projectSlug: string;
  sprintId: number;
  sprintName: string;
  sprintSlug: string;
  estimatedStart: string;
  estimatedFinish: string;
  userStories: UserStoryPreview[];
}

export interface UserStoryPreview {
  id: number;
  name: string;
  statusName: string;
  isClosed: boolean;
  userStoryRef: number;
  totalPoints: number;
}
