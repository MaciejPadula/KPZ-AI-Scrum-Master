import { TestBed } from '@angular/core/testing';

import { ProjectListDataService } from './project-list-data.service';

describe('ProjectListDataService', () => {
  let service: ProjectListDataService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ProjectListDataService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
