import BookingTable from './BookingTable';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import * as BookingService from './BookingService';
import * as RoomService from '../RoomService';

// Viser oversikten over alle bookinger.
const BookingListPage = () => {

    // Henter bookingene fra databasen.
    const { data: bookings = [], isLoading, error } = useQuery({
        queryKey: ['bookings'],
        queryFn: BookingService.fetchBookings
    });

    // Henter romdata til bookingoversikten.
    const {
        data: rooms = [],
        isLoading: roomsLoading,
        error: roomsError
    } = useQuery({
        queryKey: ['rooms', 'all'],
        queryFn: RoomService.fetchRooms
    });

    const queryClient = useQueryClient();

    // Oppdaterer bookinglisten etter vellykket sletting.
    const deleteMutation = useMutation({
        mutationFn: BookingService.deleteBooking,
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['bookings'] });
        }
    });

    // Ber om bekreftelse før bookingen slettes.
    const handleDelete = (bookingId) => {
        const confirmed = window.confirm(
            'Er du sikker på at du vil slette denne bookingen?'
        );

        if (!confirmed) {
            return;
        }

        deleteMutation.mutate(bookingId);
    };

    if (isLoading || roomsLoading)
        return <p>Laster bookinginformasjon …</p>;

    if (error || roomsError)
        return <p className="text-danger">Kunne ikke hente bookinginformasjonen.</p>;

    return (
        <div className="text-start">
            <h1>Oversikt over dine bookinger</h1>
            {deleteMutation.isError && (
                <p className="text-danger" role="alert">
                    {deleteMutation.error.message}
                </p>
            )}
            {/* Viser melding dersom det ikke finnes bookinger. */}
            {bookings.length === 0 ? (
                <p>Det finnes ingen bookinger ennå.</p>
            ) : (
                <BookingTable
                    bookings={bookings}
                    rooms={rooms}
                    onBookingDeleted={handleDelete}
                />
            )}
        </div>
    );
};

export default BookingListPage;
