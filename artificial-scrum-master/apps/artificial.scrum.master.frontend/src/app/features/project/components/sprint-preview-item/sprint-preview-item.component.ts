import {
  ChangeDetectionStrategy,
  Component,
  computed,
  input,
} from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { SprintPreview } from '../../models/sprint-preview';
import { AvatarComponent } from '../../../../shared/components/avatar/avatar.component';
import { MaterialModule } from '../../../../shared/material.module';
import { TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'app-sprint-preview-item',
  standalone: true,
  imports: [
    CommonModule,
    AvatarComponent,
    MaterialModule,
    RouterModule,
    TranslateModule,
  ],
  templateUrl: './sprint-preview-item.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SprintPreviewItemComponent {
  public sprintElement = input.required<SprintPreview>();
  public storiesUrl = computed(() => ['/UserStories', this.sprintElement().sprintId]);
  public storiesQueryParams = computed(() => {
    return {
      project: this.sprintElement().projectId
    };
  });

  public sprintUrl = computed(() => {
    const base = 'https://tree.taiga.io/project';
    return `${base}/${this.sprintElement().projectSlug}/taskboard/${
      this.sprintElement().sprintSlug
    }`;
  });

  public formatDate(dateString: string): string {
    const date = new Date(dateString);
    const day = date.getDate().toString().padStart(2, '0');
    const month = (date.getMonth() + 1).toString().padStart(2, '0');
    const year = date.getFullYear();
    return `${day}.${month}.${year}`;
  }
}
