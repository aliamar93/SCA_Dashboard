$(function () {

    // ⭐ Main initializer (called once per table)
    function initColumnSettings(tableSelector) {
        let listSelector =IntegrateLastColumn(tableSelector);
        let table = $(tableSelector).DataTable();

        buildColumnList(table, listSelector);
        applyStoredVisibility(table, listSelector, tableSelector);
        enableColumnToggle(table, listSelector, tableSelector);

        return table;
    }

    function IntegrateLastColumn(tableSelector) {

        let tableId = $(tableSelector).attr("id");  // get table ID
        let listId = `columnList-${tableId}`;        // unique list ID


        $(tableSelector).find("thead th:last")
            .append(`
                                                <div class="dropdown" id="columnSettings">
                                                    <a href="javascript:;" class="btn btn-xs btn-outline-light btn-icon dropdown-toggle" data-bs-toggle="dropdown" data-offset="0,5"><em class="icon ni ni-setting"></em></a>
                                                    <div class="dropdown-menu dropdown-menu-xs dropdown-menu-end">
                                                        <ul class="link-tidy sm no-bdr" id="${listId}">
                                                            
                                                        </ul>
                                                    </div>
                                                </div>
                                            `);

        return `#${listId}`;
    }

    // ⭐ Build checkbox list from DataTable header
    function buildColumnList(table, listContainer) {
        $(listContainer).empty(); // Clear old items

        table.columns().every(function () {
            let colIndex = this.index();
            let colTitle = $(this.header()).text().trim();
            if (colTitle !== "") {
                $(listContainer).append(`
                    <li>
                       <div class="custom-control custom-control-sm custom-checkbox">
                           <input type="checkbox" class="custom-control-input toggle-col" 
                                  data-col="${colIndex}" checked 
                                  id="col-${colIndex}">
                           <label class="custom-control-label" for="col-${colIndex}">${colTitle}</label>
                       </div>
                    </li>
                `);
            }
        });
    }

    // ⭐ Save column visibility to localStorage
    function saveToLocal(tableSelector, listSelector) {
        let settings = {};
        $(`${listSelector} .toggle-col`).each(function () {
            settings[$(this).data('col')] = $(this).is(':checked');
        });

        localStorage.setItem(`columns_${tableSelector}`, JSON.stringify(settings));
    }

    // ⭐ Restore saved visibility when loading page
    function applyStoredVisibility(table, listSelector, tableSelector) {
        let saved = JSON.parse(localStorage.getItem(`columns_${tableSelector}`));

        if (!saved) return;

        $.each(saved, function (colIndex, visible) {
            table.column(colIndex).visible(visible);
            $(`${listSelector} .toggle-col[data-col="${colIndex}"]`).prop("checked", visible);
        });
    }

    // ⭐ Enable column toggle behavior
    function enableColumnToggle(table, listSelector, tableSelector) {
        $(document).on("change", `${listSelector} .toggle-col`, function () {
            debugger;
            let colIndex = $(this).data("col");
            table.column(colIndex).visible($(this).is(":checked"));
            saveToLocal(tableSelector, listSelector);
        });
    }

    // ⭐ Expose function globally
    window.initColumnSettings = initColumnSettings;
});
