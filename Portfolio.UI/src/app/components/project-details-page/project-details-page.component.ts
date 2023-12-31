import { Component, ElementRef, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { ProjectResponse } from 'src/app/models/project-response.model';
import { ImagesService } from 'src/app/services/images.service';
import { ProjectsService } from 'src/app/services/projects.service';


@Component({
  selector: 'app-project-details-page',
  templateUrl: './project-details-page.component.html',
  styleUrls: ['./project-details-page.component.css']
})
export class ProjectDetailsComponent implements OnInit {

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private projectService: ProjectsService,
    private elRef: ElementRef,
    private imagesService: ImagesService,
    private spinner: NgxSpinnerService) {
  }
  
  projectResponse: ProjectResponse | undefined;
  imageUrl!: string;
  projectId = this.route.snapshot.paramMap.get('projectId');

  ngOnInit(): void {
    this.getProject();
  }

  getProject() {
    this.projectService.getProjectById(Number(this.projectId)).subscribe({
      next: (response) => {
        if (response) {
          console.log(response)
          this.projectResponse = response;
        }
      }
    }
    );
  }

  deleteProject() {
    this.spinner.show();
    this.projectService.deleteProjectById(Number(this.projectId)).subscribe({
      next: () => {
        this.spinner.hide();
        this.router.navigate(['/projects']);
      }
    }
    );
  }
}
