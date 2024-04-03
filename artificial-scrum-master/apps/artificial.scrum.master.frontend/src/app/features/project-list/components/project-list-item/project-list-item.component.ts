import { Component, inject, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { GetUserProjectsResponseElement } from '../../models/user-project';
import { LinkComponent } from '../../../../shared/components/link/link.component';
import { MaterialModule } from '../../../../shared/material.module';

@Component({
  selector: 'app-project-list-item',
  standalone: true,
  imports: [CommonModule, LinkComponent, MaterialModule],
  templateUrl: './project-list-item.component.html',
})
export class ProjectListItemComponent {
  @Input()
  public userProject: GetUserProjectsResponseElement;

  private readonly router = inject(Router);

  public formatDate(date: Date): string {
    const day = String(date.getDate()).padStart(2, '0');
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const year = date.getFullYear();
    return `${day}.${month}.${year}`;
  }

  onComponentClick(id: number) {
    this.router.navigate([`/projects/${id}`]);
  }
}
