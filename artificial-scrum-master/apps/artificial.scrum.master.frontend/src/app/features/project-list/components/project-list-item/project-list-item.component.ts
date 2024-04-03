import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
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

  public getObjectUrl(id: number) {
    return `/test/${id}`;
  }
}
