import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { SprintPreview } from '../../models/sprint-preview';
import { AvatarComponent } from '../../../../shared/components/avatar/avatar.component';
import { MaterialModule } from '../../../../shared/material.module';

@Component({
  selector: 'app-sprint-preview-item',
  standalone: true,
  imports: [CommonModule, AvatarComponent, MaterialModule, RouterModule],
  templateUrl: './sprint-preview-item.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SprintPreviewItemComponent {
  @Input()
  public sprintElement: SprintPreview;

  public formatDate(dateString: string): string {
    const date = new Date(dateString);
    const day = date.getDate().toString().padStart(2, '0');
    const month = (date.getMonth() + 1).toString().padStart(2, '0');
    const year = date.getFullYear();
    return `${day}.${month}.${year}`;
  }

  public get storiesUrl() {
    return `/UserStories/${this.sprintElement.sprintId}?project=${this.sprintElement.projectId}`;
  }

  public get scrumServiceUrl() {
    const base = 'https://tree.taiga.io/project';
    return `${base}/${this.sprintElement.projectSlug}/taskboard/${this.sprintElement.sprintSlug}`;
  }
}
