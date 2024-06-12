import {
  ChangeDetectionStrategy,
  Component,
  computed,
  inject,
  input,
} from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { Sprint } from '../../../sprints/models/sprint';
import { AvatarComponent } from '../../../../shared/components/avatar/avatar.component';
import { MaterialModule } from '../../../../shared/material.module';
import { TranslateModule } from '@ngx-translate/core';
import { RetroSessionComponent } from '../retro-session/retro-session.component';
import { MatDialog } from '@angular/material/dialog';
import { SprintStoriesPriorityComponent } from '../sprint-stories-priority/sprint-stories-priority.component';

@Component({
  selector: 'app-sprint-preview-item',
  standalone: true,
  templateUrl: './sprint-preview-item.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [
    CommonModule,
    AvatarComponent,
    MaterialModule,
    RouterModule,
    TranslateModule,
    RetroSessionComponent,
  ],
})
export class SprintPreviewItemComponent {
  public sprintElement = input.required<Sprint>();
  public storiesUrl = computed(() => [
    '/UserStories',
    this.sprintElement().sprintId,
  ]);
  public storiesQueryParams = computed(() => {
    return {
      project: this.sprintElement().projectId,
    };
  });

  public sprintUrl = computed(() => {
    const base = 'https://tree.taiga.io/project';
    return `${base}/${this.sprintElement().projectSlug}/taskboard/${
      this.sprintElement().sprintSlug
    }`;
  });

  #dialog = inject(MatDialog);

  public formatDate(dateString: string): string {
    const date = new Date(dateString);
    const day = date.getDate().toString().padStart(2, '0');
    const month = (date.getMonth() + 1).toString().padStart(2, '0');
    const year = date.getFullYear();
    return `${day}.${month}.${year}`;
  }

  public openStoryPriorityEditor(event: Event): void {
    event.stopPropagation();
    event.preventDefault();

    this.#dialog.open(SprintStoriesPriorityComponent, {
      data: {
        projectId: this.sprintElement().projectId,
        sprintId: this.sprintElement().sprintId,
      },
      panelClass: 'popup',
      maxWidth: '100dvw',
    });
  }
}
