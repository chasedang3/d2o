import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzInputModule } from 'ng-zorro-antd/input';
import { Task } from '../../models/task.model';
import { TaskService } from '../../services/task.service';
import { NzModalService } from 'ng-zorro-antd/modal';
import { NzModalModule } from 'ng-zorro-antd/modal';
import { NzPaginationModule } from 'ng-zorro-antd/pagination';


@Component({
  selector: 'app-task',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    HttpClientModule,
    NzButtonModule,
    NzInputModule,
    NzModalModule,
    NzPaginationModule,
  ],
  templateUrl: './task.component.html',
  styleUrls: ['./task.component.css'],
})
export class TaskComponent {
  taskName: string = '';
  tasks: Task[] = [];
  filter: 'all' | 'completed' | 'pending' = 'all';
  currentPage: number = 1;
  pageSize: number = 5;

  constructor(private taskService: TaskService, private modal: NzModalService) {
    this.taskService.tasks$.subscribe((tasks) => {
      this.tasks = tasks;
      this.resetPagination();
    });
  }

  addTask() {
    if (this.taskName.trim()) {
      this.taskService.addTask(this.taskName).subscribe((newTask) => {
        this.taskName = '';
        this.resetPagination();
      });
    }
  }


  deleteTask(id: string) {
    this.modal.confirm({
      nzTitle: 'Are you sure you want to delete this task?',
      nzOnOk: () => {
        this.taskService.deleteTask(id);
        this.resetPagination();
      },
    });
  }

  toggleCompletion(task: Task) {
    this.taskService.updateTask(task.id, task.name, !task.isCompleted);
    this.resetPagination();
  }

  filterTasks() {
    switch (this.filter) {
      case 'completed':
        return this.tasks.filter((task) => task.isCompleted);
      case 'pending':
        return this.tasks.filter((task) => !task.isCompleted);
      default:
        return this.tasks;
    }
  }

  paginatedTasks() {
    const filteredTasks = this.filterTasks();
    const startIndex = (this.currentPage - 1) * this.pageSize;
    return filteredTasks.slice(startIndex, startIndex + this.pageSize);
  }

  onPageChange(page: number) {
    this.currentPage = page;
  }

  changeFilter(newFilter: 'all' | 'completed' | 'pending') {
    this.filter = newFilter;
    this.resetPagination();
  }

  resetPagination() {
    this.currentPage = 1;
  }
}
