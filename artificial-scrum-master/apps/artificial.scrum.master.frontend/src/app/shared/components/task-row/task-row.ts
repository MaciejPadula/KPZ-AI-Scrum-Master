export interface TaskRow {
  taskId: number;
  subject: string;
  taskRef: number;
  totalComments: number;
  tags: string[][];
  statusName: string;
  statusColor: string;
  assignedToUsername: string | null;
  assignedToFullName: string | null;
  assignedToPhoto: string | null;
  isClosed: boolean;
}
