export interface GetSprintStats {
  sprintName: string | null;
  estimatedStart: string | null;
  estimatedFinish: string | null;
  totalUserStories: number;
  completedUserStories: number;
  totalTasks: number;
  completedTasks: number;
  sprintDayStats: SprintDayStats[];
  totalRolePoints: RolePointsKeyValuePair[];
  completedRolePoints: number;
}

export interface SprintDayStats {
  day: string;
  name: number;
  openPoints: number;
  optimalPoints: number;
}

export interface RolePointsKeyValuePair {
  key: string;
  value: number;
}
