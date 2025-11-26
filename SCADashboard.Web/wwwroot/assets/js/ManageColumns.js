//<style>
//    .btn-teal {
//        background - color: #0a6f63;
//    color: white;
//        }

//    .btn-teal:hover {
//        background - color: #06574e;
//    color: white;
//            }

//    .sortable-list .list-group-item {
//        cursor: grab;
//        }
//</style>


//<div class="modal fade" id="manageColumnsModal" tabindex="-1">
//    <div class="modal-dialog modal-xl modal-dialog-centered">
//        <div class="modal-content">
//            <div class="modal-header border-0">
//                <h5 class="modal-title">Manage columns</h5>
//                <button class="btn-close" data-bs-dismiss="modal"></button>
//            </div>

//            <div class="modal-body">

//                <div class="row g-4">

//                    <!-- LEFT: Available columns -->
//                    <div class="col-md-6">
//                        <div class="card shadow-sm border-0">
//                            <div class="card-body">
//                                <h6 class="fw-bold mb-3">Available columns:</h6>

//                                <div class="d-flex justify-content-between mb-2">
//                                    <span class="text-muted" id="leftCount">0 items selected</span>
//                                </div>

//                                <!-- Search -->
//                                <div class="input-group mb-3">
//                                    <input type="text" class="form-control" placeholder="Search...">
//                                        <span class="input-group-text">
//                                            <i class="bi bi-search"></i>
//                                        </span>
//                                </div>

//                                <!-- List -->
//                                <div class="list-group border rounded p-2" style="height: 300px; overflow-y: auto;">
//                                    <label class="list-group-item">
//                                        <input type="checkbox" class="form-check-input me-2">
//                                            Description
//                                    </label>

//                                    <label class="list-group-item">
//                                        <input type="checkbox" class="form-check-input me-2">
//                                            Quantity
//                                    </label>

//                                    <label class="list-group-item">
//                                        <input type="checkbox" class="form-check-input me-2">
//                                            Visualization
//                                    </label>

//                                    <label class="list-group-item">
//                                        <input type="checkbox" class="form-check-input me-2">
//                                            Length
//                                    </label>

//                                    <label class="list-group-item">
//                                        <input type="checkbox" class="form-check-input me-2">
//                                            Cost
//                                    </label>
//                                </div>
//                            </div>
//                        </div>
//                    </div>

//                    <!-- MIDDLE BUTTONS -->
//                    <div class="col-md-1 d-flex flex-column justify-content-center align-items-center">
//                        <button class="btn btn-teal mb-3 w-100">Add →</button>
//                        <button class="btn btn-outline-secondary w-100">← Remove</button>
//                    </div>

//                    <!-- RIGHT: Display in this order -->
//                    <div class="col-md-5">
//                        <div class="card shadow-sm border-0">
//                            <div class="card-body">
//                                <h6 class="fw-bold mb-3">Display in this order:</h6>

//                                <div class="d-flex justify-content-between mb-2">
//                                    <span class="text-muted">5 items</span>
//                                </div>

//                                <!-- Search -->
//                                <div class="input-group mb-3">
//                                    <input type="text" class="form-control" placeholder="Search...">
//                                        <span class="input-group-text">
//                                            <i class="bi bi-search"></i>
//                                        </span>
//                                </div>

//                                <!-- Sortable List -->
//                                <ul class="list-group sortable-list p-0" style="overflow-y: auto; height: 300px;">
//                                    <li class="list-group-item d-flex align-items-center">
//                                        <i class="bi bi-grip-vertical me-2 text-secondary"></i>
//                                        <input type="checkbox" class="form-check-input me-2">
//                                            Thumbnail
//                                    </li>

//                                    <li class="list-group-item d-flex align-items-center">
//                                        <i class="bi bi-grip-vertical me-2 text-secondary"></i>
//                                        <input type="checkbox" class="form-check-input me-2">
//                                            Status
//                                    </li>

//                                    <li class="list-group-item d-flex align-items-center">
//                                        <i class="bi bi-grip-vertical me-2 text-secondary"></i>
//                                        <input type="checkbox" class="form-check-input me-2">
//                                            Creator
//                                    </li>

//                                    <li class="list-group-item d-flex align-items-center">
//                                        <i class="bi bi-grip-vertical me-2 text-secondary"></i>
//                                        <input type="checkbox" class="form-check-input me-2">
//                                            Material
//                                    </li>
//                                </ul>

//                            </div>
//                        </div>
//                    </div>

//                </div>
//            </div>

//            <!-- Footer -->
//            <div class="modal-footer border-0">
//                <button class="btn btn-outline-secondary" data-bs-dismiss="modal">Cancel</button>
//                <button class="btn btn-teal">Update</button>
//            </div>
//        </div>
//    </div>
//</div>






$(function () {

    let dt; // DataTable instance
    let tableId = "transportunit"; // change only this or make dynamic

    // 1️⃣ Initialize DataTable
    function initTable() {
        dt = $("#" + tableId).DataTable();
        buildLeftSide();
        buildRightSide();
        restoreState();
        initSortable();
    }

    // 2️⃣ Build "Available columns"
    function buildLeftSide() {
        $("#availableList").html("");

        dt.columns().every(function () {
            const index = this.index();
            const title = this.header().innerText.trim();

            $("#availableList").append(`
                <label class="list-group-item">
                    <input type="checkbox" class="form-check-input me-2 col-left" data-col="${index}">
                    ${title}
                </label>
            `);
        });
    }

    // 3️⃣ Build "Display in this order" list
    function buildRightSide() {
        $("#selectedList").html("");

        dt.columns().every(function () {
            const index = this.index();
            const title = this.header().innerText.trim();

            $("#selectedList").append(`
                <li class="list-group-item d-flex align-items-center" data-col="${index}">
                    <i class="bi bi-grip-vertical me-2 text-secondary"></i>
                    <input type="checkbox" checked class="form-check-input me-2 col-right">
                    ${title}
                </li>
            `);
        });
    }

    // 4️⃣ Make the right-side list sortable
    function initSortable() {
        $("#selectedList").sortable({
            handle: ".bi-grip-vertical"
        });
    }

    // 5️⃣ Add → move selected from left to right
    $("#btnAdd").click(function () {
        $("#availableList .col-left:checked").each(function () {
            const col = $(this).data("col");
            const title = $(this).parent().text().trim();

            $("#selectedList").append(`
                <li class="list-group-item d-flex align-items-center" data-col="${col}">
                    <i class="bi bi-grip-vertical me-2 text-secondary"></i>
                    <input type="checkbox" class="form-check-input me-2 col-right" checked>
                    ${title}
                </li>
            `);

            $(this).closest("label").remove();
        });
    });

    // 6️⃣ Remove ← selected from right to left
    $("#btnRemove").click(function () {
        $("#selectedList .col-right:checked").each(function () {
            const parent = $(this).closest("li");
            const col = parent.data("col");
            const title = parent.text().trim();

            $("#availableList").append(`
                <label class="list-group-item">
                    <input type="checkbox" class="form-check-input me-2 col-left" data-col="${col}">
                    ${title}
                </label>
            `);

            parent.remove();
        });
    });

    // 7️⃣ Save settings to localStorage
    function saveState() {
        let state = {
            visibleColumns: [],
            order: []
        };

        // visibility (right side always visible)
        $("#selectedList li").each(function () {
            state.visibleColumns.push($(this).data("col"));
        });

        // order list
        $("#selectedList li").each(function () {
            state.order.push($(this).data("col"));
        });

        localStorage.setItem("datatable_" + tableId, JSON.stringify(state));
    }

    // 8️⃣ Restore saved settings
    function restoreState() {
        const saved = localStorage.getItem("datatable_" + tableId);
        if (!saved) return;

        const state = JSON.parse(saved);

        // Apply visibility
        dt.columns().every(function () {
            const colIndex = this.index();
            this.visible(state.visibleColumns.includes(colIndex));
        });

        // Apply order
        state.order.forEach((colIndex, newPosition) => {
            dt.colReorder.move(colIndex, newPosition);
        });
    }

    // 9️⃣ On Update button
    $("#btnUpdate").click(function () {
        saveState();
        location.reload(); // refresh table with updated ordering
    });

    // 🔟 Open modal
    $("#openColumnManager").click(function () {
        $("#manageColumnsModal").modal("show");
    });

    // Initialize everything
    initTable();
});
