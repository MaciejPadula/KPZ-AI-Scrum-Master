import {
  Component,
  Input,
  ChangeDetectionStrategy,
  inject,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserStory } from '../../models/user-story';
import { AvatarComponent } from '../../../../shared/components/avatar/avatar.component';
import { MaterialModule } from '../../../../shared/material.module';
import { MatDialog } from '@angular/material/dialog';
import { UserStoryDetailsComponent } from '../user-story-details/user-story-details.component';
import { TranslateModule } from '@ngx-translate/core';
import { TasksComponent } from '../../../tasks/tasks.component';
import { EditorStateService } from '../../../../shared/services/editor-state.service';

@Component({
  selector: 'app-user-story-list-item',
  standalone: true,
  templateUrl: './user-story-list-item.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [
    CommonModule,
    AvatarComponent,
    MaterialModule,
    TranslateModule,
    TasksComponent,
  ],
})
export class UserStoryListItemComponent {
  @Input()
  public userStory: UserStory;

  @Input()
  public projectId: number;

  public panelOpenState = false;

  #dialog = inject(MatDialog);
  private readonly editorStateServiceService = inject(EditorStateService);

  public openDetails(): void {
    const dialogRef = this.#dialog.open(UserStoryDetailsComponent, {
      data: {
        userStoryId: this.userStory.userStoryId,
        projectId: this.projectId,
      },
      panelClass: 'popup',
      maxWidth: '100dvw',
    });

    dialogRef.afterClosed().subscribe(() => {
      this.editorStateServiceService.resetEditorState();
    });
  }
}
