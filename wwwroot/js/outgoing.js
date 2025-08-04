$(document).ready(function () {
    const rowsPerPage = 6;
    let currentPage = 1;
    let totalRecords = 0;

    const loadingHtml = '<tr><td colspan="7" style="text-align:center;"><i class="fas fa-spinner fa-spin" aria-label="Loading data"></i> กำลังโหลดข้อมูล...</td></tr>';
    const errorHtml = '<tr><td colspan="7" style="text-align:center;">เกิดข้อผิดพลาดในการโหลดข้อมูล กรุณาลองใหม่อีกครั้ง</td></tr>';
    const noDataHtml = '<tr><td colspan="7" style="text-align:center;">ไม่พบข้อมูล</td></tr>';

    const tableBody = $('#outgoingTable tbody');
    const pageInfo = $('#pageInfo');
    const prevPageBtn = $('#prevPage');
    const nextPageBtn = $('#nextPage');

    async function fetchOutgoingData() {
        tableBody.html(loadingHtml);

        try {
            const response = await $.ajax({
                url: '/Outgoing/GetPaginatedOutgoingData',
                method: 'GET',
                dataType: 'json',
                data: {
                    pageNumber: currentPage,
                    pageSize: rowsPerPage
                }
            });

            if (response.error) {
                console.error("Error from server:", response.message);
                tableBody.html(errorHtml);
                return;
            }

            const outgoingData = response.data || [];
            totalRecords = response.totalRecords || 0;

            renderTable(outgoingData);
            updatePaginationControls();
        } catch (error) {
            console.error("เกิดข้อผิดพลาดในการดึงข้อมูล Outgoing:", error);
            tableBody.html(errorHtml);
        }
    }

    function renderTable(dataToRender) {
        tableBody.empty();

        if (dataToRender.length === 0) {
            tableBody.append(noDataHtml);
            return;
        }

        const rowsHtml = dataToRender.map(item => {
            const statusInfo = getStatusDisplayInfo(item.status);
            return `
                <tr>
                    <td>${item.productId}</td>
                    <td>${item.productName || '-'}</td>
                    <td>${item.category || '-'}</td>
                    <td>${item.customerName || '-'}</td>
                    <td>${item.orderedQuantity}</td>
                    <td>${item.scheduledShipDate || '-'}</td>
                    <td>
                        <span class="status-cell ${statusInfo.className}">
                            <span class="status-text">${item.status}</span>
                        </span>
                    </td>
                </tr>
            `;
        }).join('');

        tableBody.append(rowsHtml);
    }

    function getStatusDisplayInfo(status) {
        let className = 'status-default';
        switch (status.toLowerCase()) {
            case 'pending':
                className = 'status-pending';
                break;
            case 'shipped':
                className = 'status-shipped';
                break;
            case 'delivered':
                className = 'status-delivered';
                break;
            case 'cancelled':
                className = 'status-cancelled';
                break;
        }
        return {
            className
        };
    }

    function updatePaginationControls() {
        const totalPages = Math.ceil(totalRecords / rowsPerPage);
        pageInfo.text(`หน้า ${currentPage} จาก ${totalPages}`);
        prevPageBtn.prop('disabled', currentPage === 1);
        nextPageBtn.prop('disabled', currentPage >= totalPages || totalPages === 0);
    }

    prevPageBtn.on('click', function () {
        if (currentPage > 1) {
            currentPage--;
            fetchOutgoingData();
        }
    });

    nextPageBtn.on('click', function () {
        const totalPages = Math.ceil(totalRecords / rowsPerPage);
        if (currentPage < totalPages) {
            currentPage++;
            fetchOutgoingData();
        }
    });

    fetchOutgoingData();
});