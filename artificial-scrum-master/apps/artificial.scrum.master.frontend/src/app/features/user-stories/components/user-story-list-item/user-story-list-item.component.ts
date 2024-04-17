import {
  Component,
  Input,
  ChangeDetectionStrategy,
  inject,
  ViewChild,
  AfterViewInit,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserStory } from '../../models/user-story';
import { Router } from '@angular/router';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-user-story-list-item',
  standalone: true,
  imports: [
    CommonModule,
    MatExpansionModule,
    MatSort,
    MatSortModule,
    MatTableModule,
    MatIconModule,
  ],
  templateUrl: './user-story-list-item.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UserStoryListItemComponent implements AfterViewInit {
  @Input()
  public userStory: UserStory;

  @ViewChild(MatSort) sort: MatSort;

  private readonly router = inject(Router);

  displayedColumns: string[] = ['id', 'name', 'progress', 'fruit'];
  dataSource = new MatTableDataSource<UserStory>();

  constructor() {
    //this.dataSource = new MatTableDataSource(this.userStory.tas);
  }

  ngAfterViewInit(): void {
    this.dataSource.sort = this.sort;
  }
}
