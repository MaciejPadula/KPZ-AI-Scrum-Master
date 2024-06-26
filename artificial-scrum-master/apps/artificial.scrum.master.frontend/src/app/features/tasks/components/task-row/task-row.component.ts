import { Component, inject, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TaskRow } from '../../models/task-row';
import { AvatarComponent } from '../../../../shared/components/avatar/avatar.component';
import { TranslateModule } from '@ngx-translate/core';
import { MatDialog } from '@angular/material/dialog';
import { MaterialModule } from '../../../../shared/material.module';
import { TaskDetailsComponent } from '../task-details/task-details.component';
import { EditorStateService } from '../../../../shared/services/editor-state.service';

@Component({
  selector: 'app-task-row',
  standalone: true,
  imports: [
    CommonModule,
    AvatarComponent,
    MaterialModule,
    TranslateModule,
    MaterialModule,
  ],
  templateUrl: './task-row.component.html',
})
export class TaskRowComponent {
  @Input()
  public taskRow: TaskRow;

  #dialog = inject(MatDialog);
  private readonly editorStateServiceService = inject(EditorStateService);

  public openDetails(): void {
    const dialogRef = this.#dialog.open(TaskDetailsComponent, {
      data: this.taskRow.taskId,
      panelClass: 'popup',
      maxWidth: '100dvw',
    });

    dialogRef.afterClosed().subscribe(() => {
      this.editorStateServiceService.resetEditorState();
    });
  }
}
