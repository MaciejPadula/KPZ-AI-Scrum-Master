import { ScrumObjectState } from "../../models/scrum-object-state";
import { ScrumObjectType } from "../../models/scrum-object-type";

export interface TimelineRow {
  userName: string;
  userNick: string | null;
  userAvatar: string;

  scrumObjectType: ScrumObjectType;
  scrumObjectId: number;
  scrumObjectName: string;
  scrumObjectState: ScrumObjectState;

  projectId: number | null;
  projectName: string | null;
}