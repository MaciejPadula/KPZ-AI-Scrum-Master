import {
  ChangeDetectionStrategy,
  Component,
  inject,
  Input,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { UserProject } from '../../models/user-project';
import { LinkComponent } from '../../../../shared/components/link/link.component';
import { MaterialModule } from '../../../../shared/material.module';
import { TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'app-project-list-item',
  standalone: true,
  imports: [CommonModule, LinkComponent, MaterialModule, TranslateModule],
  templateUrl: './project-list-item.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ProjectListItemComponent {
  @Input()
  public userProject: UserProject;

  private readonly router = inject(Router);

  public formatDate(date: string): string {
    return new Date(date)
      .toLocaleString(navigator.language, {
        day: '2-digit',
        month: '2-digit',
        year: 'numeric',
        hour: '2-digit',
        minute: '2-digit',
        hour12: false,
      })
      .replace(',', ' at');
  }

  redirectToProject(id: number) {
    this.router.navigate([`/Project/${id}`]);
  }

  public get isOwnerKey(): string {
    return this.userProject.amOwner ? 'Shared.Words.Yes' : 'Shared.Words.No';
  }

  public get visibilityKey(): string {
    return this.userProject.isPrivate
      ? 'Shared.Words.Private'
      : 'Shared.Words.Public';
  }
}
